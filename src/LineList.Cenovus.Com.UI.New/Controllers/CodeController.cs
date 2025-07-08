using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Code;  // DTOs for Code
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class CodeController : Controller
    {
        private readonly ICodeService _codeService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public CodeController(ICodeService codeService, IMapper mapper, CurrentUser currentUser)
        {
            _codeService = codeService ?? throw new ArgumentNullException(nameof(codeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var codes = await _codeService.GetAll();
            var codeDtos = _mapper.Map<IEnumerable<CodeResultDto>>(codes);
            return View(codeDtos);
        }

        [HttpGet]
        public async Task<JsonResult> CodeFeed()
        {
            var codes = await _codeService.GetAll();
            var codeDtos = _mapper.Map<IEnumerable<CodeResultDto>>(codes);
            return Json(new { data = codeDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CodeAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(CodeAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var code = _mapper.Map<Code>(model);
            var newCode = await _codeService.Add(code);

            if (newCode == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var code = await _codeService.GetById(id);
            if (code == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_codeService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Operating Mode", "Code", code.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var codeDto = _mapper.Map<CodeEditDto>(code);
            return PartialView("_Update", codeDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(CodeEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var code = _mapper.Map<Code>(model);
            await _codeService.Update(code);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var code = await _codeService.GetById(id);
            if (code == null)
                return Json(new { success = false, ErrorMessage = "Code not found" });

            await _codeService.Remove(code);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentCode = await _codeService.GetById(request.Id);

            if (currentCode == null)
                return Json(new { success = false, ErrorMessage = "Code not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the Code to swap with (higher for move down, lower for move up)
            var swapCode = (await _codeService.GetAll())
                .Where(c => isMoveUp ? c.SortOrder < currentCode.SortOrder : c.SortOrder > currentCode.SortOrder)
                .OrderBy(c => isMoveUp ? c.SortOrder * -1 : c.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapCode == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No Code to move up." : "No Code to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentCode.SortOrder;

            currentCode.SortOrder = swapCode.SortOrder;

            swapCode.SortOrder = tempSortOrder;

            // Update both records
            await _codeService.Update(currentCode);

            await _codeService.Update(swapCode);

            return Json(new { success = true });
        }
    }
}
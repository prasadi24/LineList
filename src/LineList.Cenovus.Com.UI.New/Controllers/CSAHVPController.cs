using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.CsaHvpLvp;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
     [Authorize(Policy = "Admin")]
    public class CSAHVPController : Controller
    {
        private readonly ICsaHvpLvpService _csahvpService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;


        public CSAHVPController(ICsaHvpLvpService csahvpService, IMapper mapper, CurrentUser currentUser)
        {
            _csahvpService = csahvpService ?? throw new ArgumentNullException(nameof(csahvpService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        // GET: List of CSAHVPs
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var csahvpList = await _csahvpService.GetAll();
            var csahvpDtos = _mapper.Map<IEnumerable<CsaHvpLvpResultDto>>(csahvpList);
            return View(csahvpDtos);
        }

        // GET: Get CSAHVP Feed (for data table or API responses)
        [HttpGet]
        public async Task<JsonResult> CSAHVPFeed()
        {
            var csahvpList = await _csahvpService.GetAll();
            var csahvpDtos = _mapper.Map<IEnumerable<CsaHvpLvpResultDto>>(csahvpList);
            return Json(new { data = csahvpDtos });
        }

        // GET: Create CSAHVP
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CsaHvpLvpAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        // POST: Create CSAHVP
        [HttpPost]
        public async Task<JsonResult> Create(CsaHvpLvpAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var csahvp = _mapper.Map<CsaHvpLvp>(model);
            var newCsahvp = await _csahvpService.Add(csahvp);

            if (newCsahvp == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        // GET: Edit CSAHVP
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var csahvp = await _csahvpService.GetById(id);
            if (csahvp == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_csahvpService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Operating Mode", "CSA HVP/LVP", csahvp.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var csahvpDto = _mapper.Map<CsaHvpLvpEditDto>(csahvp);
            return PartialView("_Update", csahvpDto);
        }

        // POST: Update CSAHVP
        [HttpPost]
        public async Task<JsonResult> Update(CsaHvpLvpEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var csahvp = _mapper.Map<CsaHvpLvp>(model);
            await _csahvpService.Update(csahvp);

            return Json(new { success = true });
        }

        // DELETE: Delete CSAHVP
        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var csahvp = await _csahvpService.GetById(id);
            if (csahvp == null)
                return Json(new { success = false, ErrorMessage = "CSAHVP not found" });

            await _csahvpService.Remove(csahvp);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentCSAHVP = await _csahvpService.GetById(request.Id);

            if (currentCSAHVP == null)
                return Json(new { success = false, ErrorMessage = "CSAHVP not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the CSAHVP to swap with (higher for move down, lower for move up)
            var swapCSAHVP = (await _csahvpService.GetAll())
                .Where(c => isMoveUp ? c.SortOrder < currentCSAHVP.SortOrder : c.SortOrder > currentCSAHVP.SortOrder)
                .OrderBy(c => isMoveUp ? c.SortOrder * -1 : c.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapCSAHVP == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No CSAHVP to move up." : "No CSAHVP to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentCSAHVP.SortOrder;

            currentCSAHVP.SortOrder = swapCSAHVP.SortOrder;

            swapCSAHVP.SortOrder = tempSortOrder;

            // Update both records
            await _csahvpService.Update(currentCSAHVP);

            await _csahvpService.Update(swapCSAHVP);

            return Json(new { success = true });
        }
    }
}
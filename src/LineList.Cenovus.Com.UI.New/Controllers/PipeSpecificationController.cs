using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.PipeSpecification;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class PipeSpecificationController : Controller
    {
        private readonly IPipeSpecificationService _pipeSpecificationService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        private readonly ISpecificationService _specificationService;
        private readonly ICorrosionAllowanceService _corrosionAllowanceService;
        private readonly INdeCategoryService _ndeCategoryService;
        private readonly IXrayService _xrayService;

        public PipeSpecificationController(IPipeSpecificationService pipeSpecificationService, IMapper mapper, ISpecificationService specificationService, ICorrosionAllowanceService corrosionAllowanceService, INdeCategoryService ndeCategoryService, IXrayService xrayService, CurrentUser currentUser)
        {
            _pipeSpecificationService = pipeSpecificationService ?? throw new ArgumentNullException(nameof(pipeSpecificationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _specificationService = specificationService;
            _corrosionAllowanceService = corrosionAllowanceService;
            _ndeCategoryService = ndeCategoryService;
            _xrayService = xrayService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pipeSpecifications = await _pipeSpecificationService.GetAll();
            var pipeSpecificationDtos = _mapper.Map<IEnumerable<PipeSpecificationResultDto>>(pipeSpecifications);

            return View(pipeSpecificationDtos);
        }

        [HttpGet]
        public async Task<JsonResult> PipeSpecificationFeed()
        {
            var pipeSpecifications = await _pipeSpecificationService.GetAll();
            var pipeSpecificationDtos = _mapper.Map<IEnumerable<PipeSpecificationResultDto>>(pipeSpecifications);
            return Json(new { data = pipeSpecificationDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PipeSpecificationAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName,
                RevisionDate = new DateTime(1900, 01, 01)
            };

            ViewBag.Specifications = _specificationService.GetAll().Result.Where(a => a.IsActive).OrderBy(a => a.SortOrder).ToList();
            ViewBag.CorrosionAllownaces = _corrosionAllowanceService.GetAll().Result.Where(a => a.IsActive).OrderBy(a => a.SortOrder).ToList();
            ViewBag.NdeCategaries = _ndeCategoryService.GetAll().Result.Where(a => a.IsActive).OrderBy(a => a.Name).ToList();
            ViewBag.Xrays = _xrayService.GetAll().Result.Where(a => a.IsActive).OrderBy(a => a.SortOrder).ToList();

            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(PipeSpecificationAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var pipeSpecification = _mapper.Map<PipeSpecification>(model);
            var newPipeSpecification = await _pipeSpecificationService.Add(pipeSpecification);

            if (newPipeSpecification == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var pipeSpecification = await _pipeSpecificationService.GetById(id);
            if (pipeSpecification == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_pipeSpecificationService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision, Schedule Default", "Pipe Specification", pipeSpecification.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var pipeSpecificationDto = _mapper.Map<PipeSpecificationEditDto>(pipeSpecification);

            ViewBag.Specifications = _specificationService.GetAll().Result.Where(a => a.IsActive).OrderBy(a => a.SortOrder).ToList();
            ViewBag.CorrosionAllownaces = _corrosionAllowanceService.GetAll().Result.Where(a => a.IsActive).OrderBy(a => a.SortOrder).ToList();
            ViewBag.NdeCategaries = _ndeCategoryService.GetAll().Result.Where(a => a.IsActive).OrderBy(a => a.Name).ToList();
            ViewBag.Xrays = _xrayService.GetAll().Result.Where(a => a.IsActive).OrderBy(a => a.SortOrder).ToList();

            return PartialView("_Update", pipeSpecificationDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(PipeSpecificationEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var pipeSpecification = _mapper.Map<PipeSpecification>(model);
            var updatedPipeSpecification = await _pipeSpecificationService.Update(pipeSpecification);
            if (updatedPipeSpecification == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var pipeSpecification = await _pipeSpecificationService.GetById(id);
            if (pipeSpecification == null)
                return Json(new { success = false, ErrorMessage = "PipeSpecification not found" });

            await _pipeSpecificationService.Remove(pipeSpecification);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentPipeSpecification = await _pipeSpecificationService.GetById(request.Id);
            if (currentPipeSpecification == null)
                return Json(new { success = false, ErrorMessage = "PipeSpecification not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the PipeSpecification to swap with (higher for move down, lower for move up)
            var swapPipeSpecification = (await _pipeSpecificationService.GetAll())
                .Where(ps => isMoveUp ? ps.SortOrder < currentPipeSpecification.SortOrder : ps.SortOrder > currentPipeSpecification.SortOrder)
                .OrderBy(ps => isMoveUp ? ps.SortOrder * -1 : ps.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapPipeSpecification == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No PipeSpecification to move up." : "No PipeSpecification to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentPipeSpecification.SortOrder;
            currentPipeSpecification.SortOrder = swapPipeSpecification.SortOrder;
            swapPipeSpecification.SortOrder = tempSortOrder;

            // Update both records
            await _pipeSpecificationService.Update(currentPipeSpecification);
            await _pipeSpecificationService.Update(swapPipeSpecification);

            return Json(new { success = true });
        }
    }
}
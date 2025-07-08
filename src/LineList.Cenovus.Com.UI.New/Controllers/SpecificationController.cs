using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Specification;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class SpecificationController : Controller
    {
        private readonly ISpecificationService _specificationService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;


        public SpecificationController(ISpecificationService specificationService, IMapper mapper, CurrentUser currentUser)
        {
            _specificationService = specificationService ?? throw new ArgumentNullException(nameof(specificationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var specifications = await _specificationService.GetAll();
            var specificationDtos = _mapper.Map<IEnumerable<SpecificationResultDto>>(specifications);
            return View(specificationDtos);
        }

        [HttpGet]
        public async Task<JsonResult> SpecificationFeed()
        {
            var specifications = await _specificationService.GetAll();
            var specificationDtos = _mapper.Map<IEnumerable<SpecificationResultDto>>(specifications);
            return Json(new { data = specificationDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SpecificationAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(SpecificationAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var specification = _mapper.Map<Specification>(model);
            var newSpecification = await _specificationService.Add(specification);

            if (newSpecification == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var specification = await _specificationService.GetById(id);
            if (specification == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_specificationService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Tracing Types, Line List Revision, Area, Pipe Specification, Line Revision, Commodity", "Specification", specification.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var specificationDto = _mapper.Map<SpecificationEditDto>(specification);
            return PartialView("_Update", specificationDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(SpecificationEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var specification = _mapper.Map<Specification>(model);
            var updateSpecification = await _specificationService.Update(specification);

            if (updateSpecification == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var specification = await _specificationService.GetById(id);
            if (specification == null)
                return Json(new { success = false, ErrorMessage = "Specification not found" });

            await _specificationService.Remove(specification);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentSpecification = await _specificationService.GetById(request.Id);
            if (currentSpecification == null)
                return Json(new { success = false, ErrorMessage = "Specification not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the specification to swap with (higher for move down, lower for move up)
            var swapSpecification = (await _specificationService.GetAll())
                .Where(s => isMoveUp ? s.SortOrder < currentSpecification.SortOrder : s.SortOrder > currentSpecification.SortOrder)
                .OrderBy(s => isMoveUp ? s.SortOrder * -1 : s.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapSpecification == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No specification to move up." : "No specification to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentSpecification.SortOrder;
            currentSpecification.SortOrder = swapSpecification.SortOrder;
            swapSpecification.SortOrder = tempSortOrder;

            // Update both records
            await _specificationService.Update(currentSpecification);
            await _specificationService.Update(swapSpecification);

            return Json(new { success = true });
        }
    }
}
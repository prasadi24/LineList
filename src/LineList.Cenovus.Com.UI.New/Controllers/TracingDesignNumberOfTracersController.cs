using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.TracingDesignNumberOfTracers;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class TracingDesignNumberOfTracersController : Controller
    {
        private readonly ITracingDesignNumberOfTracersService _tracingDesignNumberOfTracersService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;


        public TracingDesignNumberOfTracersController(ITracingDesignNumberOfTracersService tracingDesignNumberOfTracersService, IMapper mapper, CurrentUser currentUser)
        {
            _tracingDesignNumberOfTracersService = tracingDesignNumberOfTracersService ?? throw new ArgumentNullException(nameof(tracingDesignNumberOfTracersService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tracingDesignNumberOfTracers = await _tracingDesignNumberOfTracersService.GetAll();
            var tracingDesignNumberOfTracersDtos = _mapper.Map<IEnumerable<TracingDesignNumberOfTracersResultDto>>(tracingDesignNumberOfTracers);
            return View(tracingDesignNumberOfTracersDtos);
        }

        [HttpGet]
        public async Task<JsonResult> TracingDesignNumberOfTracersFeed()
        {
            var tracingDesignNumberOfTracers = await _tracingDesignNumberOfTracersService.GetAll();
            var tracingDesignNumberOfTracersDtos = _mapper.Map<IEnumerable<TracingDesignNumberOfTracersResultDto>>(tracingDesignNumberOfTracers);
            return Json(new { data = tracingDesignNumberOfTracersDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new TracingDesignNumberOfTracersAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(TracingDesignNumberOfTracersAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var tracingDesignNumberOfTracers = _mapper.Map<TracingDesignNumberOfTracers>(model);
            var newTracingDesignNumberOfTracers = await _tracingDesignNumberOfTracersService.Add(tracingDesignNumberOfTracers);
            if (newTracingDesignNumberOfTracers == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var tracingDesignNumberOfTracers = await _tracingDesignNumberOfTracersService.GetById(id);
            if (tracingDesignNumberOfTracers == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_tracingDesignNumberOfTracersService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Segment, Insulation Default Detail", "Tracing Design Tracer", tracingDesignNumberOfTracers.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var tracingDesignNumberOfTracersDto = _mapper.Map<TracingDesignNumberOfTracersEditDto>(tracingDesignNumberOfTracers);
            return PartialView("_Update", tracingDesignNumberOfTracersDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(TracingDesignNumberOfTracersEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var tracingDesignNumberOfTracers = _mapper.Map<TracingDesignNumberOfTracers>(model);
            await _tracingDesignNumberOfTracersService.Update(tracingDesignNumberOfTracers);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var tracingDesignNumberOfTracers = await _tracingDesignNumberOfTracersService.GetById(id);
            if (tracingDesignNumberOfTracers == null)
                return Json(new { success = false, ErrorMessage = "TracingDesignNumberOfTracers not found" });

            await _tracingDesignNumberOfTracersService.Remove(tracingDesignNumberOfTracers);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentTracingDesignNumberOfTracers = await _tracingDesignNumberOfTracersService.GetById(request.Id);
            if (currentTracingDesignNumberOfTracers == null)
                return Json(new { success = false, ErrorMessage = "TracingDesignNumberOfTracers not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the TracingDesignNumberOfTracers to swap with (higher for move down, lower for move up)
            var swapTracingDesignNumberOfTracers = (await _tracingDesignNumberOfTracersService.GetAll())
                .Where(tdnt => isMoveUp ? tdnt.SortOrder < currentTracingDesignNumberOfTracers.SortOrder : tdnt.SortOrder > currentTracingDesignNumberOfTracers.SortOrder)
                .OrderBy(tdnt => isMoveUp ? tdnt.SortOrder * -1 : tdnt.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapTracingDesignNumberOfTracers == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No TracingDesignNumberOfTracers to move up." : "No TracingDesignNumberOfTracers to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentTracingDesignNumberOfTracers.SortOrder;
            currentTracingDesignNumberOfTracers.SortOrder = swapTracingDesignNumberOfTracers.SortOrder;
            swapTracingDesignNumberOfTracers.SortOrder = tempSortOrder;

            // Update both records
            await _tracingDesignNumberOfTracersService.Update(currentTracingDesignNumberOfTracers);
            await _tracingDesignNumberOfTracersService.Update(swapTracingDesignNumberOfTracers);

            return Json(new { success = true });
        }
    }
}
using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.ScheduleDefault;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ScheduleDefaultController : Controller
    {
        private readonly IScheduleDefaultService _scheduleDefaultService;
        private readonly IPipeSpecificationService _pipeSpecificationService;
        private readonly ISizeNpsService _sizeNpsService;
        private readonly IScheduleService _scheduleService;
        private readonly ISpecificationService _specificationService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public ScheduleDefaultController(IScheduleDefaultService scheduleDefaultService, IPipeSpecificationService pipeSpecificationService, IScheduleService scheduleService, ISizeNpsService sizeNpsService, IMapper mapper, ISpecificationService specificationService, CurrentUser currentUser)
        {
            _scheduleDefaultService = scheduleDefaultService;
            _pipeSpecificationService = pipeSpecificationService;
            _scheduleService = scheduleService;
            _mapper = mapper;
            _sizeNpsService = sizeNpsService;
            _specificationService = specificationService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var schedules = await _scheduleDefaultService.GetAll();
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleDefaultResultDto>>(schedules);
            return View(scheduleDtos);
        }

        [HttpGet]
        public async Task<JsonResult> ScheduleDefaultFeed()
        {
            var schedules = await _scheduleDefaultService.GetAll();
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleDefaultResultDto>>(schedules);
            return Json(new { data = scheduleDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var schedules = await _scheduleService.GetAll();
            ViewBag.Schdules = schedules
                .Where(s => s.IsActive)
                .OrderBy(s => s.SortOrder);

            var pipeSpecs = await _pipeSpecificationService.GetAll();
            ViewBag.PipeSpecifications = pipeSpecs
                .Where(p => p.IsActive)
                .OrderBy(p => p.SortOrder);

            var npsList = await _sizeNpsService.GetAll();
            ViewBag.SizeNps = npsList
                .Where(n => n.IsActive)
                .OrderBy(n => n.SortOrder);

            var specs = await _specificationService.GetAll();
            ViewBag.Specifications = specs
                .Where(sp => sp.IsActive)
                .OrderBy(sp => sp.SortOrder);
            var nowMST = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time")
    );
            var model = new ScheduleDefaultAddDto
            {
                SortOrder = 0,
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName,
                RevisionDate = nowMST
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(ScheduleDefaultAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var schedule = _mapper.Map<ScheduleDefault>(model);
            await _scheduleDefaultService.Add(schedule);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            ViewBag.Schdules = _scheduleService.GetAll().Result.Where(r => r.IsActive).OrderBy(r => r.SortOrder);
            ViewBag.PipeSpecifications = _pipeSpecificationService.GetAll().Result.Where(r => r.IsActive).OrderBy(r => r.SortOrder);
            ViewBag.SizeNps = _sizeNpsService.GetAll().Result.Where(r => r.IsActive).OrderBy(r => r.SortOrder);
            ViewBag.Specifications = _specificationService.GetAll().Result.Where(r => r.IsActive).OrderBy(r => r.SortOrder);

            var schedule = await _scheduleDefaultService.GetById(id);
            if (schedule == null)
                return NotFound();

            var scheduleDto = _mapper.Map<ScheduleDefaultEditDto>(schedule);
            scheduleDto.SpecificationId = schedule.PipeSpecification.Specification.Id;
            return PartialView("_Update", scheduleDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(ScheduleDefaultEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var schedule = _mapper.Map<ScheduleDefault>(model);
            await _scheduleDefaultService.Update(schedule);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var schedule = await _scheduleDefaultService.GetById(id);
            if (schedule == null)
                return Json(new { success = false, ErrorMessage = "Schedule Default not found" });

            await _scheduleDefaultService.Remove(schedule);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentSchedule = await _scheduleDefaultService.GetById(request.Id);
            if (currentSchedule == null)
                return Json(new { success = false, ErrorMessage = "Schedule Default not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            var swapSchedule = (await _scheduleDefaultService.GetAll())
                .Where(s => isMoveUp ? s.SortOrder < currentSchedule.SortOrder : s.SortOrder > currentSchedule.SortOrder)
                .OrderBy(s => isMoveUp ? s.SortOrder * -1 : s.SortOrder)
                .FirstOrDefault();

            if (swapSchedule == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No schedule to move up." : "No schedule to move down." });

            int? tempSortOrder = currentSchedule.SortOrder;
            currentSchedule.SortOrder = swapSchedule.SortOrder;
            swapSchedule.SortOrder = tempSortOrder;

            await _scheduleDefaultService.Update(currentSchedule);
            await _scheduleDefaultService.Update(swapSchedule);

            return Json(new { success = true });
        }
    }
}
using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Schedule;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]

    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public ScheduleController(IScheduleService scheduleService, IMapper mapper, CurrentUser currentUser)
        {
            _scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var schedules = await _scheduleService.GetAll();
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleResultDto>>(schedules);
            return View(scheduleDtos);
        }

        [HttpGet]
        public async Task<JsonResult> ScheduleFeed()
        {
            var schedules = await _scheduleService.GetAll();
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleResultDto>>(schedules);
            return Json(new { data = scheduleDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ScheduleAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(ScheduleAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var schedule = _mapper.Map<Schedule>(model);
            var newSchedule = await _scheduleService.Add(schedule);
            if (newSchedule == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var schedule = await _scheduleService.GetById(id);
            if (schedule == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_scheduleService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision, Schedule Default", "Schedule", schedule.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var scheduleDto = _mapper.Map<ScheduleEditDto>(schedule);
            return PartialView("_Update", scheduleDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(ScheduleEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var schedule = _mapper.Map<Schedule>(model);
            await _scheduleService.Update(schedule);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var schedule = await _scheduleService.GetById(id);
            if (schedule == null)
                return Json(new { success = false, ErrorMessage = "Schedule not found" });

            await _scheduleService.Remove(schedule);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentSchedule = await _scheduleService.GetById(request.Id);
            if (currentSchedule == null)
                return Json(new { success = false, ErrorMessage = "Schedule not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the Schedule to swap with (higher for move down, lower for move up)
            var swapSchedule = (await _scheduleService.GetAll())
                .Where(s => isMoveUp ? s.SortOrder < currentSchedule.SortOrder : s.SortOrder > currentSchedule.SortOrder)
                .OrderBy(s => isMoveUp ? s.SortOrder * -1 : s.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapSchedule == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No Schedule to move up." : "No Schedule to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentSchedule.SortOrder;
            currentSchedule.SortOrder = swapSchedule.SortOrder;
            swapSchedule.SortOrder = tempSortOrder;

            // Update both records
            await _scheduleService.Update(currentSchedule);
            await _scheduleService.Update(swapSchedule);

            return Json(new { success = true });
        }
    }
}
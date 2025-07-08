using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.StressAnalysis;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class StressAnalysisController : Controller
    {
        private readonly IStressAnalysisService _stressAnalysisService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        public StressAnalysisController(IStressAnalysisService stressAnalysisService, IMapper mapper, CurrentUser currentUser)
        {
            _stressAnalysisService = stressAnalysisService ?? throw new ArgumentNullException(nameof(stressAnalysisService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var stressAnalyses = await _stressAnalysisService.GetAll();
            var stressAnalysisDtos = _mapper.Map<IEnumerable<StressAnalysisResultDto>>(stressAnalyses);
            return View(stressAnalysisDtos);
        }

        [HttpGet]
        public async Task<JsonResult> StressAnalysisFeed()
        {
            var stressAnalyses = await _stressAnalysisService.GetAll();
            var stressAnalysisDtos = _mapper.Map<IEnumerable<StressAnalysisResultDto>>(stressAnalyses);
            return Json(new { data = stressAnalysisDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new StressAnalysisAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(StressAnalysisAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var stressAnalysis = _mapper.Map<StressAnalysis>(model);
            var newStressAnalysis = await _stressAnalysisService.Add(stressAnalysis);

            if (newStressAnalysis == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var stressAnalysis = await _stressAnalysisService.GetById(id);
            if (stressAnalysis == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_stressAnalysisService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision", "Stress Analysis", stressAnalysis.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var stressAnalysisDto = _mapper.Map<StressAnalysisEditDto>(stressAnalysis);
            return PartialView("_Update", stressAnalysisDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(StressAnalysisEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var stressAnalysis = _mapper.Map<StressAnalysis>(model);
            await _stressAnalysisService.Update(stressAnalysis);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var stressAnalysis = await _stressAnalysisService.GetById(id);
            if (stressAnalysis == null)
                return Json(new { success = false, ErrorMessage = "Stress Analysis not found" });

            await _stressAnalysisService.Remove(stressAnalysis);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentStressAnalysis = await _stressAnalysisService.GetById(request.Id);

            if (currentStressAnalysis == null)
                return Json(new { success = false, ErrorMessage = "StressAnalysis not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the StressAnalysis to swap with (higher for move down, lower for move up)
            var swapStressAnalysis = (await _stressAnalysisService.GetAll())
                .Where(sa => isMoveUp ? sa.SortOrder < currentStressAnalysis.SortOrder : sa.SortOrder > currentStressAnalysis.SortOrder)
                .OrderBy(sa => isMoveUp ? sa.SortOrder * -1 : sa.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapStressAnalysis == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No StressAnalysis to move up." : "No StressAnalysis to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentStressAnalysis.SortOrder;

            currentStressAnalysis.SortOrder = swapStressAnalysis.SortOrder;

            swapStressAnalysis.SortOrder = tempSortOrder;

            // Update both records
            await _stressAnalysisService.Update(currentStressAnalysis);

            await _stressAnalysisService.Update(swapStressAnalysis);

            return Json(new { success = true });
        }
    }
}
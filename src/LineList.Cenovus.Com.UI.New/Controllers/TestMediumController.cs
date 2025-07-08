using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.TestMedium;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class TestMediumController : Controller
    {
        private readonly ITestMediumService _testMediumService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public TestMediumController(ITestMediumService testMediumService, IMapper mapper, CurrentUser currentUser)
        {
            _testMediumService = testMediumService ?? throw new ArgumentNullException(nameof(testMediumService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var testMediums = await _testMediumService.GetAll();
            var testMediumDtos = _mapper.Map<IEnumerable<TestMediumResultDto>>(testMediums);
            return View(testMediumDtos);
        }

        [HttpGet]
        public async Task<JsonResult> TestMediumFeed()
        {
            var testMediums = await _testMediumService.GetAll();
            var testMediumDtos = _mapper.Map<IEnumerable<TestMediumResultDto>>(testMediums);
            return Json(new { data = testMediumDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new TestMediumAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(TestMediumAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var testMedium = _mapper.Map<TestMedium>(model);
            var newTestMedium = await _testMediumService.Add(testMedium);

            if (newTestMedium == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var testMedium = await _testMediumService.GetById(id);
            if (testMedium == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_testMediumService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision", "Test Medium", testMedium.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var testMediumDto = _mapper.Map<TestMediumEditDto>(testMedium);
            return PartialView("_Update", testMediumDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(TestMediumEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var testMedium = _mapper.Map<TestMedium>(model);
            await _testMediumService.Update(testMedium);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var testMedium = await _testMediumService.GetById(id);
            if (testMedium == null)
                return Json(new { success = false, ErrorMessage = "TestMedium not found" });

            await _testMediumService.Remove(testMedium);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentTestMedium = await _testMediumService.GetById(request.Id);
            if (currentTestMedium == null)
                return Json(new { success = false, ErrorMessage = "TestMedium not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the TestMedium to swap with (higher for move down, lower for move up)
            var swapTestMedium = (await _testMediumService.GetAll())
                .Where(tm => isMoveUp ? tm.SortOrder < currentTestMedium.SortOrder : tm.SortOrder > currentTestMedium.SortOrder)
                .OrderBy(tm => isMoveUp ? tm.SortOrder * -1 : tm.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapTestMedium == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No TestMedium to move up." : "No TestMedium to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentTestMedium.SortOrder;
            currentTestMedium.SortOrder = swapTestMedium.SortOrder;
            swapTestMedium.SortOrder = tempSortOrder;

            // Update both records
            await _testMediumService.Update(currentTestMedium);
            await _testMediumService.Update(swapTestMedium);

            return Json(new { success = true });
        }
    }
}
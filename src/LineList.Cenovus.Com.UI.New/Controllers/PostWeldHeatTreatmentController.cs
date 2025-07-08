using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.PostWeldHeatTreatment;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class PostWeldHeatTreatmentController : Controller
    {
        private readonly IPostWeldHeatTreatmentService _postWeldHeatTreatmentService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public PostWeldHeatTreatmentController(IPostWeldHeatTreatmentService postWeldHeatTreatmentService, IMapper mapper, CurrentUser currentUser)
        {
            _postWeldHeatTreatmentService = postWeldHeatTreatmentService ?? throw new ArgumentNullException(nameof(postWeldHeatTreatmentService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var postWeldHeatTreatments = await _postWeldHeatTreatmentService.GetAll();
            var postWeldHeatTreatmentDtos = _mapper.Map<IEnumerable<PostWeldHeatTreatmentResultDto>>(postWeldHeatTreatments);
            return View(postWeldHeatTreatmentDtos);
        }

        [HttpGet]
        public async Task<JsonResult> PostWeldHeatTreatmentFeed()
        {
            var postWeldHeatTreatments = await _postWeldHeatTreatmentService.GetAll();
            var postWeldHeatTreatmentDtos = _mapper.Map<IEnumerable<PostWeldHeatTreatmentResultDto>>(postWeldHeatTreatments);
            return Json(new { data = postWeldHeatTreatmentDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PostWeldHeatTreatmentAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(PostWeldHeatTreatmentAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var postWeldHeatTreatment = _mapper.Map<PostWeldHeatTreatment>(model);
            var newPostWeldHeatTreatment = await _postWeldHeatTreatmentService.Add(postWeldHeatTreatment);

            if (newPostWeldHeatTreatment == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var postWeldHeatTreatment = await _postWeldHeatTreatmentService.GetById(id);
            if (postWeldHeatTreatment == null)
                return NotFound();
            var canDel = true;
            string message = "";
            if (_postWeldHeatTreatmentService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Pipe Specification, Line Revision", "Post Weld Heat Treatment", postWeldHeatTreatment.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

              canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var postWeldHeatTreatmentDto = _mapper.Map<PostWeldHeatTreatmentEditDto>(postWeldHeatTreatment);
            return PartialView("_Update", postWeldHeatTreatmentDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(PostWeldHeatTreatmentEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var postWeldHeatTreatment = _mapper.Map<PostWeldHeatTreatment>(model);
            await _postWeldHeatTreatmentService.Update(postWeldHeatTreatment);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var postWeldHeatTreatment = await _postWeldHeatTreatmentService.GetById(id);
            if (postWeldHeatTreatment == null)
                return Json(new { success = false, ErrorMessage = "PostWeldHeatTreatment not found" });

            string message = "";
            if (_postWeldHeatTreatmentService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Pipe Specification, Line Revision", "Post Weld Heat Treatment", postWeldHeatTreatment.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                return Json(new { success = false, ErrorMessage = message });
            }

            await _postWeldHeatTreatmentService.Remove(postWeldHeatTreatment);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentPostWeldHeatTreatment = await _postWeldHeatTreatmentService.GetById(request.Id);

            if (currentPostWeldHeatTreatment == null)
                return Json(new { success = false, ErrorMessage = "PostWeldHeatTreatment not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the PostWeldHeatTreatment to swap with (higher for move down, lower for move up)
            var swapPostWeldHeatTreatment = (await _postWeldHeatTreatmentService.GetAll())
                .Where(pwht => isMoveUp ? pwht.SortOrder < currentPostWeldHeatTreatment.SortOrder : pwht.SortOrder > currentPostWeldHeatTreatment.SortOrder)
                .OrderBy(pwht => isMoveUp ? pwht.SortOrder * -1 : pwht.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapPostWeldHeatTreatment == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No PostWeldHeatTreatment to move up." : "No PostWeldHeatTreatment to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentPostWeldHeatTreatment.SortOrder;

            currentPostWeldHeatTreatment.SortOrder = swapPostWeldHeatTreatment.SortOrder;

            swapPostWeldHeatTreatment.SortOrder = tempSortOrder;

            // Update both records
            await _postWeldHeatTreatmentService.Update(currentPostWeldHeatTreatment);

            await _postWeldHeatTreatmentService.Update(swapPostWeldHeatTreatment);

            return Json(new { success = true });
        }
    }
}
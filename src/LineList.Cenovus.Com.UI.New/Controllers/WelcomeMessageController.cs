using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.WelcomeMessage;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class WelcomeMessageController : Controller
    {
        private readonly IWelcomeMessageService _welcomeMessageService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public WelcomeMessageController(IWelcomeMessageService welcomeMessageService, IMapper mapper, CurrentUser currentUser)
        {
            _welcomeMessageService = welcomeMessageService ?? throw new ArgumentNullException(nameof(welcomeMessageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        // GET: WelcomeMessage
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var welcomeMessages = await _welcomeMessageService.GetAll();
            var welcomeMessageDtos = _mapper.Map<IEnumerable<WelcomeMessageResultDto>>(welcomeMessages);
            return View(welcomeMessageDtos.FirstOrDefault());
        }

        // GET: WelcomeMessage/Feed (For DataTables or other feeds)
        [HttpGet]
        public async Task<JsonResult> WelcomeMessageFeed()
        {
            var welcomeMessages = await _welcomeMessageService.GetAll();
            var welcomeMessageDtos = _mapper.Map<IEnumerable<WelcomeMessageResultDto>>(welcomeMessages);
            return Json(new { data = welcomeMessageDtos });
        }

        // GET: WelcomeMessage/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new WelcomeMessageAddDto
            {
                CreatedBy = _currentUser.FullName,  
                ModifiedBy = _currentUser.FullName  
            };
            return PartialView("_Create", model);
        }

        // POST: WelcomeMessage/Create
        [HttpPost]
        public async Task<JsonResult> Create(WelcomeMessageAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var welcomeMessage = _mapper.Map<WelcomeMessage>(model);
            await _welcomeMessageService.Add(welcomeMessage);

            return Json(new { success = true });
        }

        // GET: WelcomeMessage/Update/{id}
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var welcomeMessage = await _welcomeMessageService.GetById(id);
            if (welcomeMessage == null)
                return NotFound();

            var welcomeMessageDto = _mapper.Map<WelcomeMessageEditDto>(welcomeMessage);
            welcomeMessageDto.CreatedBy = string.IsNullOrWhiteSpace(welcomeMessage.CreatedBy) ? _currentUser.FullName : welcomeMessage.CreatedBy;
            welcomeMessageDto.CreatedOn = welcomeMessage.CreatedOn != default ? welcomeMessage.CreatedOn : DateTime.Now;
            welcomeMessageDto.ModifiedBy = _currentUser.FullName;
            welcomeMessageDto.ModifiedOn = DateTime.Now;
            return PartialView("_Update", welcomeMessageDto);
        }

        //POST: WelcomeMessage/Update
       [HttpPost]
        public async Task<JsonResult> Update(WelcomeMessageEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var welcomeMessage = _mapper.Map<WelcomeMessage>(model);
            await _welcomeMessageService.Update(welcomeMessage);

            return Json(new { success = true });
        }

        // DELETE: WelcomeMessage/Delete/{id}
        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var welcomeMessage = await _welcomeMessageService.GetById(id);
            if (welcomeMessage == null)
                return Json(new { success = false, ErrorMessage = "WelcomeMessage not found" });

            await _welcomeMessageService.Remove(welcomeMessage);
            return Json(new { success = true });
        }
    }
}

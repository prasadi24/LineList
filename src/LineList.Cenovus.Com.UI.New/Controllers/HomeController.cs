using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.WelcomeMessage;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Security;
using LineList.Cenovus.Com.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IWelcomeMessageService _welcomeMessageService;
        private readonly ILineListModelService _lineListModelService;
        private readonly ILineService _lineService;
        private readonly IMapper _mapper;
        private readonly SessionTracker _sessionTracker;

        private readonly UserManager _userManager;
        private readonly CurrentUser _currentUser;

        private readonly IConfiguration _configuration;

        public HomeController(IWelcomeMessageService welcomeMessageService,
            ILineListModelService lineListModelService,
            ILineService lineService,
            IMapper mapper,
            SessionTracker sessionTracker,
            UserManager userManager,
            CurrentUser currentUser,
            IConfiguration configuration)
        {
            _welcomeMessageService = welcomeMessageService;
            _lineListModelService = lineListModelService;
            _lineService = lineService;
            _mapper = mapper;
            _sessionTracker = sessionTracker;
            _userManager = userManager;
            _currentUser = currentUser;
            _configuration = configuration;
        }

        // GET: WelcomeMessage
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fullName = _currentUser.FullName;
            var isAdmin = _currentUser.IsCenovusAdmin;  
            var isReadonly = _currentUser.IsReadOnly;
            var welcomeMessages = await _welcomeMessageService.GetAll();
            var lineLists = _lineListModelService.GetCount();
            var lines = _lineService.GetCount();
            var avgLines = lineLists == 0 ? 0 : (lines / lineLists);
            string lastUpdate = _configuration["SiteSettings:LastUpdate"];
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                WelcomeMessage = welcomeMessages.FirstOrDefault(),
                TotalLineList = Convert.ToString(lineLists),
                AverageLines = Convert.ToString(avgLines),
                TotalUsers = Convert.ToString(_sessionTracker.GetActiveSessionCount()),
                LastUpdated = lastUpdate,
                IsReadOnly = isReadonly,
                IsCenovusAdmin = isAdmin
            };
            var welcomeMessageDtos = _mapper.Map<IEnumerable<WelcomeMessageResultDto>>(welcomeMessages);
            return View(homeViewModel);
        }

        /// <summary>
        /// Redirects to the logout screen
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/"
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Unauthorized()
        {
            ViewData["Title"] = "Access Denied";
            var lineLists = _lineListModelService.GetCount();
            var lines = _lineService.GetCount();
            var avgLines = lineLists == 0 ? 0 : (lines / lineLists);
            string lastUpdate = _configuration["SiteSettings:LastUpdate"];
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                TotalLineList = Convert.ToString(lineLists),
                AverageLines = Convert.ToString(avgLines),
                TotalUsers = Convert.ToString(_sessionTracker.GetActiveSessionCount()),
                LastUpdated = lastUpdate
            };
            return View(homeViewModel);
        }
    }
}
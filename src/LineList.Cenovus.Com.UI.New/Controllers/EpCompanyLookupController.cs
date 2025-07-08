using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.EpCompany;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
     [Authorize(Policy = "Admin")]
    public class EpCompanyLookupController : Controller
    {
        private readonly IEpCompanyService _epCompanyService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IFacilityService _facilityService;
        IEpCompanyAlphaService _epCompanyAlphaService;

        public EpCompanyLookupController(IEpCompanyService epCompanyService, IMapper mapper, CurrentUser currentUser, IFacilityService facilityService, IEpCompanyAlphaService epCompanyAlphaService)
        {
            _epCompanyService = epCompanyService ?? throw new ArgumentNullException(nameof(epCompanyService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
            _facilityService = facilityService;
            _epCompanyAlphaService = epCompanyAlphaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var epCompanies = await _epCompanyService.GetAll();
            var epCompanyDtos = _mapper.Map<IEnumerable<EpCompanyResultDto>>(epCompanies);
            return View(epCompanyDtos);
        }

        [HttpGet]
        public async Task<JsonResult> EpCompanyFeed()
        {
            var epCompanies = await _epCompanyService.GetAll();
            var epCompanyDtos = _mapper.Map<IEnumerable<EpCompanyResultDto>>(epCompanies);
            return Json(new { data = epCompanyDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new EpCompanyAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpGet("EpCompanyLookup/CreateFacility/{id}")]
        public IActionResult CreateFacility(string id)
        {
            var facilities = (_facilityService.GetAll()).Result.Where(s => s.IsActive).OrderBy(s => s.SortOrder).ToList();
            var epCompanies = _epCompanyService.GetAll().Result.Where(e => e.IsActive).OrderBy(s => s.SortOrder).ToList();
            EpCompanyFacilityAddViewModel model = new EpCompanyFacilityAddViewModel()
            {
                Facilities = facilities,
                EpCompanies = epCompanies,
                EpCompanyId =new Guid(id)
            };

            return PartialView("_CreateAlpha", model);
        }

        [HttpPost]
        public async Task<JsonResult> AddFacilityAccess(EpCompanyFacilityAddViewModel model)
        {
           
            EpCompanyAlpha alpha = new EpCompanyAlpha()
            {
                FacilityId = model.FacilityId,
                EpCompanyId = model.EpCompanyId,
                Alpha = "A",
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName,
                CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time")),
                ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"))
            };

            var result = await _epCompanyAlphaService.Add(alpha);

            if (result == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> Create(EpCompanyAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var epCompany = _mapper.Map<EpCompany>(model);
            var newEpCompany = await _epCompanyService.Add(epCompany);

            if (newEpCompany == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var epCompany = await _epCompanyService.GetById(id);
            if (epCompany == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_epCompanyService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing EP Company Alpha, EP Project", "EP Company", epCompany.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var epCompanyDto = _mapper.Map<EpCompanyEditDto>(epCompany);
            List<EpCompanyAlpha> alphas = _epCompanyAlphaService.GetAll().Result.Where(a => a.EpCompanyId == id).ToList();
            ViewData["Alphas"] = alphas;
            return PartialView("_Update", epCompanyDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(EpCompanyEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var epCompany = _mapper.Map<EpCompany>(model);
            await _epCompanyService.Update(epCompany);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var epCompany = await _epCompanyService.GetById(id);
            if (epCompany == null)
                return Json(new { success = false, ErrorMessage = "EP Company not found" });
           
            await _epCompanyService.Remove(epCompany);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentEpCompany = await _epCompanyService.GetById(request.Id);
            if (currentEpCompany == null)
                return Json(new { success = false, ErrorMessage = "EpCompany not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the EpCompany to swap with (higher for move down, lower for move up)
            var swapEpCompany = (await _epCompanyService.GetAll())
                .Where(e => isMoveUp ? e.SortOrder < currentEpCompany.SortOrder : e.SortOrder > currentEpCompany.SortOrder)
                .OrderBy(e => isMoveUp ? e.SortOrder * -1 : e.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapEpCompany == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No EpCompany to move up." : "No EpCompany to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentEpCompany.SortOrder;
            currentEpCompany.SortOrder = swapEpCompany.SortOrder;
            swapEpCompany.SortOrder = tempSortOrder;

            // Update both records
            await _epCompanyService.Update(currentEpCompany);
            await _epCompanyService.Update(swapEpCompany);

            return Json(new { success = true });
        }

    }
}
using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.EpCompany;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class EpCompanyController : Controller
    {
        private readonly IEpCompanyService _epCompanyService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public EpCompanyController(IEpCompanyService epCompanyService, IMapper mapper, CurrentUser currentUser)
        {
            _epCompanyService = epCompanyService ?? throw new ArgumentNullException(nameof(epCompanyService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var companies = await _epCompanyService.GetAll();
            var companyDtos = _mapper.Map<IEnumerable<EpCompanyResultDto>>(companies);
            return View(companyDtos);
        }

        [HttpGet]
        public async Task<JsonResult> EpCompanyFeed()
        {
            var companies = await _epCompanyService.GetAll();
            var companyDtos = _mapper.Map<IEnumerable<EpCompanyResultDto>>(companies);
            return Json(new { data = companyDtos });
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

        [HttpPost]
        public async Task<JsonResult> Create(EpCompanyAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var company = _mapper.Map<EpCompany>(model);
            await _epCompanyService.Add(company);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var company = await _epCompanyService.GetById(id);
            if (company == null)
                return NotFound();            

            var companyDto = _mapper.Map<EpCompanyEditDto>(company);
            return PartialView("_Update", companyDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(EpCompanyEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var company = _mapper.Map<EpCompany>(model);
            await _epCompanyService.Update(company);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var company = await _epCompanyService.GetById(id);
            if (company == null)
                return Json(new { success = false, ErrorMessage = "Company not found" });

            company.Logo = null;
            await _epCompanyService.Update(company);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UploadLogo(IFormFile companyLogo, Guid companyId)
        {
            if (companyLogo == null || companyLogo.Length == 0)
            {
                return Json(new { success = false, message = "Please select an image." });
            }

            using (var memoryStream = new MemoryStream())
            {
                await companyLogo.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);

                // Save image to database (assumed method)
                var result = await _epCompanyService.UpdateCompanyLogo(companyId, base64String);

                if (result)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to upload image." });
                }
            }
        }
    }
}
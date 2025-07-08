using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class NotesConfigurationController : Controller
    {
        private readonly INotesConfigurationService _notesService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IWebHostEnvironment _env;
        private readonly IFacilityService _facilityService;
        private readonly ISpecificationService _specificationService;

        public NotesConfigurationController(
            INotesConfigurationService notesService,
            IMapper mapper,
            CurrentUser currentUser,
            IWebHostEnvironment env,
            IFacilityService facilityService,
            ISpecificationService specificationService)
        {
            _notesService = notesService;
            _mapper = mapper;
            _currentUser = currentUser;
            _env = env;
            _facilityService = facilityService;
            _specificationService = specificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dtos = await _notesService.GetAllWithNames();
            return View(dtos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new NotesConfigurationAddDto
            {
                UploadedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            ViewBag.Facilities = _facilityService.GetAll().Result.Where(i => i.IsActive).OrderBy(i => i.SortOrder).ToList();
            ViewBag.Specifications = _specificationService.GetAll().Result.Where(i => i.IsActive).OrderBy(i => i.SortOrder).ToList();
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(IFormFile uploadedFile)
        {
            var form = Request.Form;
            var model = new NotesConfigurationAddDto
            {
                FacilityId = Guid.TryParse(form["FacilityId"], out var fid) ? fid : Guid.Empty,
                SpecificationId = Guid.TryParse(form["SpecificationId"], out var sid) ? sid : Guid.Empty,
                UploadedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName,
            };
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid." });

            if (uploadedFile == null || uploadedFile.Length == 0)
                return Json(new { success = false, ErrorMessage = "File upload is required." });

            if (uploadedFile.ContentType != "application/pdf")
                return Json(new { success = false, ErrorMessage = "Only PDF files are allowed." });

            var isUnique = _notesService.GetAll().Result.Any(n => n.SpecificationId == model.SpecificationId && n.FacilityId == model.FacilityId);
            if (isUnique)
                return Json(new { success = false, ErrorMessage = "Notes Configuration for Facility and Specification combination already exist" });

            using var memoryStream = new MemoryStream();
            await uploadedFile.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            model.FileName = Path.GetFileName(uploadedFile.FileName);
            model.FileData = fileBytes;
            model.UploadedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            model.UploadedBy = _currentUser.FullName;

            var entity = _mapper.Map<NotesConfiguration>(model);
            await _notesService.Add(entity);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var cfg = await _notesService.GetById(id);
            if (cfg == null) return NotFound();

            var dto = _mapper.Map<NotesConfigurationEditDto>(cfg);
            ViewBag.Facilities = _facilityService.GetAll().Result.Where(i => i.IsActive).OrderBy(i => i.SortOrder).ToList();
            ViewBag.Specifications = _specificationService.GetAll().Result.Where(i => i.IsActive).OrderBy(i => i.SortOrder).ToList();

            return PartialView("_Update", dto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(IFormFile uploadedFile)
        {
            var form = Request.Form;
            var id = Guid.TryParse(form["Id"], out var fid) ? fid : Guid.Empty;
            if (id == Guid.Empty)
                return Json(new { success = false, ErrorMessage = "Record not found." });

            var model = await _notesService.GetById(id);
            using var memoryStream = new MemoryStream();
            await uploadedFile.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            model.FileName = Path.GetFileName(uploadedFile.FileName);
            model.FileData = fileBytes;
            model.UploadedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            model.UploadedBy = _currentUser.FullName;

            var entity = _mapper.Map<NotesConfiguration>(model);
            await _notesService.Update(entity);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> Replace(IFormFile uploadedFile)
        {
            var form = Request.Form;

            Guid.TryParse(form["Id"], out var id);

            if (uploadedFile == null || uploadedFile.Length == 0)
                return Json(new { success = false, errorMessage = "File upload is required." });

            if (uploadedFile.ContentType != "application/pdf")
                return Json(new { success = false, errorMessage = "Only PDF files are allowed." });

            var entity = await _notesService.GetById(id);
            if (entity == null)
                return Json(new { success = false, errorMessage = "Record not found." });

            using var memoryStream = new MemoryStream();
            await uploadedFile.CopyToAsync(memoryStream);

            entity.FileName = uploadedFile.FileName;
            entity.FileData = memoryStream.ToArray();
            entity.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            entity.ModifiedBy = _currentUser.FullName;

            await _notesService.Update(entity); // Ensure this exists

            return Json(new { success = true });
        }


        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var cfg = await _notesService.GetById(id);
            if (cfg == null) return Json(new { success = false, ErrorMessage = "Not found." });

            await _notesService.Remove(cfg);
            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> Download(Guid id)
        {
            var note = await _notesService.GetById(id); // Or your method
            if (note == null || note.FileData == null || note.FileData.Length == 0)
            {
                return NotFound();
            }

            var fileName = string.IsNullOrEmpty(note.FileName) ? "NotesConfiguration.pdf" : note.FileName;

            return File(note.FileData, "application/pdf", fileName);
        }
        [HttpGet]
        public async Task<IActionResult> ViewDocument(Guid id)
        {
            var file = await _notesService.GetById(id);
            if (file == null || file.FileData == null)
                return NotFound();

            return File(file.FileData, "application/pdf");
        }
    }

}

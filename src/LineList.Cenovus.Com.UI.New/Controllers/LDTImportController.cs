using LineList.Cenovus.Com.API.DataTransferObjects.ImportRow;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LineList.Cenovus.Com.UI.Controllers
{
    //[Authorize]
    public class LDTImportController : Controller
    {
        private readonly IImportService _importService;
        private readonly IFacilityService _facilityService;
        private readonly ILineListModelService _lineListService;
        private readonly ILineRevisionService _lineRevisionService;
        private readonly CurrentUser _currentUser;

        public LDTImportController(
            IImportService importService,
            IFacilityService facilityService,
            ILineListModelService lineListService,
            ILineRevisionService lineRevisionService,
            CurrentUser currentUser)
        {
            _importService = importService;
            _facilityService = facilityService;
            //_lineListService = lineListService;
            _lineRevisionService = lineRevisionService;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> Index()
        {
            //if (!_currentUser.IsCenovusAdmin)
            //{
            //    //return RedirectToAction("Index", "Home");
            //    return View();
            //}

            //var facilities = await _facilityService.GetAll();
            ////ViewBag.Facilities = facilities.Select(f => f.Name).ToList();
            //ViewBag.Facilities = facilities?.Select(f => f.Name).ToList() ?? new List<string>(); 
            //return View();
            // Always set ViewBag.Facilities to a non-null value
            var facilities = (await _facilityService.GetAll())
                .Where(f => f.IsActive)
                .OrderBy(f => f.SortOrder)
                .Select(f => f.Name)
                .ToList();

            // Fetch imports (since the view likely renders them directly)
            var imports = await _importService.GetAll();

            // Pass facilities to the view via ViewBag
            ViewBag.Facilities = facilities;

            // Return the imports as the model
            return View(imports);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!_currentUser.IsCenovusAdmin)
            {
                return Json(new { success = false, error = "Unauthorized access." });
            }

            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, error = "No file uploaded." });
            }

            if (!file.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
            {
                return Json(new { success = false, error = "Only .xls files are allowed." });
            }

            var validationResult = await _importService.ValidateBeforeUpload(file);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return Json(new { success = false, error = validationResult });
            }
                var import = await _importService.Import(file, _currentUser.FullName);
                return Json(new { success = true });
            }

            [Route("LDTImport/Summary/{id}")]
        public async Task<IActionResult> Summary(Guid id)
        {
            Console.WriteLine($"Summary action called with id: {id}");
            var import = await _importService.GetImportDetails(id);
            if (import == null)
            {
                Console.WriteLine("Import not found");
                return NotFound();
            }

            return View(import);
        }

        public async Task<IActionResult> Exceptions(Guid importSheetId, Guid importId)
        {
            var sheet = await _importService.GetSheetDetails(importSheetId);
            if (sheet == null)
            {
                return NotFound();
            }
            if (!_currentUser.IsCenovusAdmin)
            {
                //return RedirectToAction("Index", "Home");
                return View(sheet);
            }

            ViewBag.ImportId = importId;
            return View(sheet);
        }

        [HttpPost]
        public async Task<IActionResult> ExportExceptions(Guid importSheetId)
        {
            if (!_currentUser.IsCenovusAdmin)
            {
                return Unauthorized();
            }

            var sheet = await _importService.GetSheetDetails(importSheetId);
            if (sheet == null)
            {
                return NotFound();
            }

            // Build CSV content
            var csv = new StringBuilder();
            var headers = new List<string> { "Row Number", "Exception Message" };
            headers.AddRange(sheet.Columns.Select(c => c.NameInExcel));
            csv.AppendLine(string.Join(",", headers.Select(h => $"\"{h.Replace("\"", "\"\"")}\"")));

            foreach (var row in sheet.ImportRows)
            {
                foreach (var exception in row.Exceptions)
                {
                    var rowData = new List<string>
                    {
                        row.RowNumber.ToString(),
                        $"\"{exception.Message.Replace("\"", "\"\"")}\""
                    };
                    foreach (var column in sheet.Columns)
                    {
                        var property = typeof(ImportRowResultDto).GetProperty(column.NameInDatabase);
                        var value = property?.GetValue(row)?.ToString() ?? "";
                        rowData.Add($"\"{value.Replace("\"", "\"\"")}\"");
                    }
                    csv.AppendLine(string.Join(",", rowData));
                }
            }

            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(csvBytes, "text/csv", "exceptions.csv");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!_currentUser.IsCenovusAdmin)
            {
                return Unauthorized();
            }

            var import = await _importService.GetById(id);
            if (import == null)
            {
                return NotFound();
            }

            await _importService.Remove(import);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CleanDatabase()
        {
            if (!_currentUser.IsCenovusAdmin)
            {
                return Unauthorized();
            }

            //await _lineListService.DeleteAllAsync();
            //await _lineRevisionService.DeleteAllAsync();

            var imports = await _importService.GetAll();
            foreach (var import in imports)
            {
                await _importService.Remove(import);
            }

            return RedirectToAction("Index");
        }
    }
}
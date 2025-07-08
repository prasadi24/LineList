using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultColumn;
using LineList.Cenovus.Com.Domain.DataTransferObjects.InsulationDefaultColumn;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.New.Controllers
{
    public class InsulationColumnController : Controller
    {
        private readonly IInsulationDefaultColumnService _insulationDefaultColumnService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public InsulationColumnController(IInsulationDefaultColumnService insulationDefaultColumnService,
            IMapper mapper, CurrentUser currentUser
            )
        {
            _insulationDefaultColumnService = insulationDefaultColumnService;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id, string type)
        {
            var insulationDefaultColumn = await _insulationDefaultColumnService.GetByInsulationDefaultId(id);
            if (insulationDefaultColumn == null)
                return NotFound();

            List<InsulationColumnDisplayDto> rows = new List<InsulationColumnDisplayDto>();
            int rowNum = 0;
            foreach (var item in insulationDefaultColumn.OrderBy(i => i.MinOperatingTemperature))
            {
                rowNum = rowNum + 1;
                InsulationColumnDisplayDto row = new InsulationColumnDisplayDto()
                {
                    Id = item.Id,
                    RowNum = rowNum,
                    MinOperatingTemperature = item.MinOperatingTemperature,
                    MaxOperatingTemperature = item.MaxOperatingTemperature,
                    InsulationDefaultId = item.InsulationDefaultId
                };
                rows.Add(row);
            }

            return PartialView("_Update", rows);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InsulationDefaultColumnEditDto model)
        {
            var allCols = await _insulationDefaultColumnService.GetByInsulationDefaultId(model.InsulationDefaultId);
            if (allCols.Any(c => c.Id != model.Id && model.MinOperatingTemperature <= c.MaxOperatingTemperature && model.MaxOperatingTemperature >= c.MinOperatingTemperature))
            {
              return Json(new
                {
                    success = false,
                    ErrorMessage = "Column range overlaps with an existing column. Please enter a non-overlapping range."
                                });
                           }

            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var insulationDefault = _mapper.Map<InsulationDefaultColumn>(model);
            await _insulationDefaultColumnService.Update(insulationDefault);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTemperature(Guid id, int MinOperatingTemperature, int MaxOperatingTemperature)
        {
            var column = await _insulationDefaultColumnService.GetById(id);
            if (column == null)
            {
                return Json(new { success = false, ErrorMessage = "Row not found." });
            }

            column.MinOperatingTemperature = MinOperatingTemperature;
            column.MaxOperatingTemperature = MaxOperatingTemperature;
            await _insulationDefaultColumnService.Update(column);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> CreateColumn(Guid InsulationDefaultId, int MinOperatingTemperature, int MaxOperatingTemperature)
        {
            var existing = await _insulationDefaultColumnService.GetByInsulationDefaultId(InsulationDefaultId);
            if (existing.Any(c => MinOperatingTemperature <= c.MaxOperatingTemperature && MaxOperatingTemperature >= c.MinOperatingTemperature))
            {
             return Json(new
                {
                    success = false,
                    errorMessage = "Column range overlaps with an existing column. Please enter a non-overlapping range."
                                });
                           }

            var column = new InsulationDefaultColumn
            {
                InsulationDefaultId = InsulationDefaultId,
                MinOperatingTemperature = MinOperatingTemperature,
                MaxOperatingTemperature = MaxOperatingTemperature,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };

            var created = await _insulationDefaultColumnService.Add(column);

            return Json(new { success = true, id = created.Id });
        }
    }
}
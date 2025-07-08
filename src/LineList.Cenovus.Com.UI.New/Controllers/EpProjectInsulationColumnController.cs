using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultColumn;
using LineList.Cenovus.Com.Domain.DataTransferObjects.InsulationDefaultColumn;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.New.Controllers
{
    public class EpProjectInsulationColumnController : Controller
    {
        private readonly IEpProjectInsulationDefaultColumnService _insulationDefaultColumnService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public EpProjectInsulationColumnController(IEpProjectInsulationDefaultColumnService insulationDefaultColumnService,
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
                    InsulationDefaultId = item.EpProjectInsulationDefaultId
                };
                rows.Add(row);
            }

            return PartialView("_Update", rows);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InsulationDefaultColumnEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var insulationDefault = _mapper.Map<EpProjectInsulationDefaultColumn>(model);
            await _insulationDefaultColumnService.Update(insulationDefault);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdateTemperature(Guid id, int MinOperatingTemperature, int MaxOperatingTemperature)
        {
            var column = _insulationDefaultColumnService.GetById(id).Result;
            if (column == null)
            {
                return Json(new { success = false, ErrorMessage = "Row not found." });
            }

            column.MinOperatingTemperature = MinOperatingTemperature;
            column.MaxOperatingTemperature = MaxOperatingTemperature;
            _insulationDefaultColumnService.Update(column);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult CreateColumn(Guid InsulationDefaultId, int MinOperatingTemperature, int MaxOperatingTemperature)
        {
            EpProjectInsulationDefaultColumn column = new EpProjectInsulationDefaultColumn();
            column.EpProjectInsulationDefaultId = InsulationDefaultId;
            column.MinOperatingTemperature = MinOperatingTemperature;
            column.MaxOperatingTemperature = MaxOperatingTemperature;

            column.CreatedOn = column.ModifiedOn = DateTime.Now;
            column.CreatedBy = column.ModifiedBy = _currentUser.FullName;

            _insulationDefaultColumnService.Add(column);

            return Json(new { success = true });
        }
    }
}
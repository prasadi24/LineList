using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultRow;
using LineList.Cenovus.Com.Domain.DataTransferObjects.InsulationDefaultRow;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.New.Controllers
{
    public class InsulationRowController : Controller
    {
        private readonly IInsulationDefaultRowService _insulationDefaultRowService;
        private readonly ISizeNpsService _sizeNpsService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public InsulationRowController(IInsulationDefaultRowService insulationDefaultRowService,
            ISizeNpsService sizeNpsService,
            IMapper mapper,
            CurrentUser currentUser
            )
        {
            _insulationDefaultRowService = insulationDefaultRowService;
            _sizeNpsService = sizeNpsService;
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
            var insulationDefaultRows = _insulationDefaultRowService.GetByInsulationDefaultId(id).Result.OrderBy(n => n.SizeNps.SortOrder);

            if (insulationDefaultRows == null)
                return NotFound();

            List<InsulationRowDisplayDto> rows = new List<InsulationRowDisplayDto>();
            int rowNum = 0;
            foreach (var item in insulationDefaultRows)
            {
                rowNum = rowNum + 1;
                InsulationRowDisplayDto row = new InsulationRowDisplayDto()
                {
                    Id = item.Id,
                    RowNum = rowNum,
                    SizeNps = item.SizeNps.Name,
                    SortOrder = item.SizeNps.SortOrder,
                    InsulationDefaultId = id
                };
                rows.Add(row);
            }

            return PartialView("_Update", rows);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InsulationDefaultRowEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var insulationDefault = _mapper.Map<InsulationDefaultRow>(model);
            await _insulationDefaultRowService.Update(insulationDefault);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRow(Guid id)
        {
            var insulationRow = await _insulationDefaultRowService.GetById(id);
            if (insulationRow == null)
                return NotFound();

            var insulationRowDto = _mapper.Map<InsulationDefaultRowEditDto>(insulationRow);
            return PartialView("_UpdateRow", insulationRowDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSizeNps(Guid id, Guid sizeNpsId, Guid insulationDefaultId)
        {
            InsulationDefaultRow row = await _insulationDefaultRowService.GetById(id);
            if (row == null)
                return Json(new { success = false, ErrorMessage = "Row not found." });
            var insulationDefaultRows = _insulationDefaultRowService.GetByInsulationDefaultId(insulationDefaultId).Result.OrderBy(n => n.SizeNps.SortOrder);
            var isExist = insulationDefaultRows.Any(i => i.SizeNpsId == sizeNpsId);
            if (isExist)
                return Json(new { success = false, ErrorMessage = "The selected Size NPS setting already exists as a row in this table." });
            row.SizeNpsId = sizeNpsId;
            _insulationDefaultRowService.Update(row);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRow(Guid sizeNpsId, Guid insulationDefaultId)
        {
            InsulationDefaultRow row = new InsulationDefaultRow();

            var insulationDefaultRows = _insulationDefaultRowService.GetByInsulationDefaultId(insulationDefaultId).Result.OrderBy(n => n.SizeNps.SortOrder);
            var isExist = insulationDefaultRows.Any(i => i.SizeNpsId == sizeNpsId);
            if (isExist)
                return Json(new { success = false, ErrorMessage = "The selected Size NPS setting already exists as a row in this table." });
            row.SizeNpsId = sizeNpsId;
            row.InsulationDefaultId = insulationDefaultId;
            row.CreatedOn = row.ModifiedOn = DateTime.Now;
            row.CreatedBy = row.ModifiedBy = _currentUser.FullName;

            _insulationDefaultRowService.Add(row);

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetSizeNpsOptions()
        {
            List<SizeNps> npss = _sizeNpsService.GetAll().Result.Where(m=>m.IsActive).OrderBy(m => m.SortOrder).ToList();
            return Json(npss.Select(n => new { id = n.Id, name = n.Name }));
        }
    }
}
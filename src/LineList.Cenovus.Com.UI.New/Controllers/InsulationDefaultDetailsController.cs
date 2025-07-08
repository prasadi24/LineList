using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultDetail;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.New.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InsulationDefaultDetailsController : Controller
    {
        private readonly IInsulationDefaultDetailService _insulationDefaultDetailService;
        private readonly IInsulationDefaultRowService _insulationDefaultRowService;
        private readonly IInsulationDefaultColumnService _insulationDefaultColumnService;
        private readonly ISizeNpsService _sizeNpsService;
        private readonly IInsulationThicknessService _insulationThicknessService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public InsulationDefaultDetailsController(IInsulationDefaultDetailService insulationDefaultDetailService,
            IInsulationDefaultRowService insulationDefaultRowService,
            IInsulationDefaultColumnService insulationDefaultColumnService,
            ISizeNpsService sizeNpsService,
            IInsulationThicknessService insulationThicknessService,
            IMapper mapper, CurrentUser currentUser)
        {
            _insulationDefaultDetailService = insulationDefaultDetailService;
            _insulationDefaultRowService = insulationDefaultRowService;
            _insulationDefaultColumnService = insulationDefaultColumnService;
            _sizeNpsService = sizeNpsService;
            _insulationThicknessService = insulationThicknessService;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id, Guid insulationDefaultId, Guid rowId, Guid columnId)
        {
            var insulationDefaultDetail = await _insulationDefaultDetailService.GetById(id);
            InsulationDefaultRow row = null;
            InsulationDefaultColumn col = null;
            if (insulationDefaultDetail == null)
            {
                row = await _insulationDefaultRowService.GetById(rowId);
                col = await _insulationDefaultColumnService.GetById(columnId);
            }
            else
            {
                row = await _insulationDefaultRowService.GetById(insulationDefaultDetail.InsulationDefaultRowId);
                col = await _insulationDefaultColumnService.GetById(insulationDefaultDetail.InsulationDefaultColumnId);
            }
            var sizeNPS_s = _sizeNpsService.GetAll().Result.Where(s => s.Id == row.SizeNpsId || s.IsActive == true).OrderBy(s => s.SortOrder).ToList();
            var insulationThicknesses = _insulationThicknessService.GetAll().Result.Where(s => s.IsActive == true).OrderBy(s => s.SortOrder).ToList();
            var tracingType = col.InsulationDefault.TracingType != null ? col.InsulationDefault.TracingType.Name : string.Empty;
            InsulationDefaultDetailsEditViewModel model = new InsulationDefaultDetailsEditViewModel()
            {
                Id = id,
                MinOperatingTemperature = col.MinOperatingTemperature.HasValue ? col.MinOperatingTemperature.Value.ToString() : string.Empty,
                MaxOperatingTemperature = col.MaxOperatingTemperature.HasValue ? col.MaxOperatingTemperature.Value.ToString() : string.Empty,
                SizeNpsId = row.SizeNpsId,
                InsulationDefaultRowId = row.Id,
                InsulationDefaultColumnId = col.Id,
                TracingType = tracingType,
                SizeNps_s = sizeNPS_s,
                insulationThicknesses = insulationThicknesses
            };
            if (insulationDefaultDetail != null)
            {
                model.CreatedBy = insulationDefaultDetail.CreatedBy;
                model.CreatedOn = insulationDefaultDetail.CreatedOn;
                model.ModifiedBy = insulationDefaultDetail.ModifiedBy;
                model.ModifiedOn = insulationDefaultDetail.ModifiedOn;
                model.InsulationThicknessId = insulationDefaultDetail.InsulationThicknessId;
            }

            return PartialView("_Update", model);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InsulationDefaultDetailEditDto model)
        {
            InsulationDefaultDetail insulationDefaultDetail = null;
            if (model.Id == Guid.Empty)
            {
                insulationDefaultDetail = new InsulationDefaultDetail()
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = _currentUser.FullName,
                    CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time")),
                    ModifiedBy = _currentUser.FullName,
                    ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time")),
                    InsulationDefaultColumnId = model.InsulationDefaultColumnId,
                    InsulationDefaultRowId = model.InsulationDefaultRowId
                };
                await _insulationDefaultDetailService.Add(insulationDefaultDetail);
            }
            else
                insulationDefaultDetail = await _insulationDefaultDetailService.GetById(model.Id);
            insulationDefaultDetail.InsulationThicknessId = model.InsulationThicknessId;
            insulationDefaultDetail.ModifiedBy = _currentUser.FullName;
            insulationDefaultDetail.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            var updatedInsulationDefaultDetail = await _insulationDefaultDetailService.Update(insulationDefaultDetail);

            if (updatedInsulationDefaultDetail == null)
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var insulationDefaultDetail = await _insulationDefaultDetailService.GetById(id);
            if (insulationDefaultDetail == null)
                return Json(new { success = false, ErrorMessage = "Insulation Default Detail not found" });

            await _insulationDefaultDetailService.Remove(insulationDefaultDetail);

            return Json(new { success = true });
        }


    }
}

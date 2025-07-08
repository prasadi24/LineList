using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefault;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultColumn;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultRow;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefault;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultColumn;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultDetail;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultRow;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InsulationDefaultController : Controller
    {
        private readonly IInsulationDefaultService _insulationDefaultService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        private readonly IInsulationMaterialService _insulationMaterialService;
        private readonly IInsulationTypeService _insulationTypeService;
        private readonly ITracingTypeService _tracingTypeService;
        private readonly IInsulationThicknessService _insulationThicknessService;
        private readonly IInsulationDefaultDetailService _insulationDefaultDetailService;
        private readonly IInsulationDefaultRowService _insulationDefaultRowService;
        private readonly IInsulationDefaultColumnService _insulationDefaultColumnService;
        private readonly ISpecificationService _specificationService;

        private readonly IEpProjectInsulationDefaultService _epProjectInsulationDefaultService;
        private readonly IEpProjectInsulationDefaultDetailService _epProjectInsulationDefaultDetailService;
        private readonly IEpProjectInsulationDefaultColumnService _epProjectInsulationDefaultColumnService;
        private readonly IEpProjectInsulationDefaultRowService _epProjectInsulationDefaultRowService;

        public InsulationDefaultController(IInsulationDefaultService insulationDefaultService,
            IMapper mapper,
            IInsulationMaterialService insulationMaterialService,
            IInsulationTypeService insulationTypeService,
            ITracingTypeService tracingTypeService,
            IInsulationThicknessService insulationThicknessService,
            IInsulationDefaultRowService insulationDefaultRowService,
            IInsulationDefaultColumnService insulationDefaultColumnService,
            IInsulationDefaultDetailService insulationDefaultDetailService,
            ISpecificationService specificationService,
            IEpProjectInsulationDefaultService epProjectInsulationDefaultService,
            IEpProjectInsulationDefaultDetailService epProjectInsulationDefaultDetailService,
            IEpProjectInsulationDefaultColumnService epProjectInsulationDefaultColumnService,
            IEpProjectInsulationDefaultRowService epProjectInsulationDefaultRowService,
            CurrentUser currentUser
            )
        {
            _insulationDefaultService = insulationDefaultService;
            _mapper = mapper;
            _insulationMaterialService = insulationMaterialService;
            _insulationTypeService = insulationTypeService;
            _tracingTypeService = tracingTypeService;
            _insulationThicknessService = insulationThicknessService;
            _insulationDefaultColumnService = insulationDefaultColumnService;
            _insulationDefaultRowService = insulationDefaultRowService;
            _insulationDefaultDetailService = insulationDefaultDetailService;
            _specificationService = specificationService;
            _epProjectInsulationDefaultService = epProjectInsulationDefaultService;
            _epProjectInsulationDefaultDetailService = epProjectInsulationDefaultDetailService;
            _epProjectInsulationDefaultColumnService = epProjectInsulationDefaultColumnService;
            _epProjectInsulationDefaultRowService = epProjectInsulationDefaultRowService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var insulationDefaults = await _insulationDefaultService.GetAll();
            var insulationDefaultDtos = _mapper.Map<IEnumerable<InsulationDefaultResultDto>>(insulationDefaults);
            return View(insulationDefaultDtos);
        }

        [HttpGet]
        public async Task<JsonResult> InsulationDefaultFeed()
        {
            var insulationDefaults = await _insulationDefaultService.GetAll();
            var insulationDefaultDtos = _mapper.Map<IEnumerable<InsulationDefaultResultDto>>(insulationDefaults);
            return Json(new { data = insulationDefaultDtos });
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(Guid id, string type = "D")
        {
            var insulationDefault = await _insulationDefaultService.GetById(id);
            if (insulationDefault == null)
            {
                return NotFound();
            }

            var rows = await _insulationDefaultRowService.GetByInsulationDefaultId(id);
            var columns = await _insulationDefaultColumnService.GetByInsulationDefaultId(id);

            var viewModel = new InsulationDefaultDetailsViewModel
            {
                InsulationDefault = _mapper.Map<InsulationDefaultResultDto>(insulationDefault),
                InsulationDefaultRows = _mapper.Map<IEnumerable<InsulationDefaultRowResultDto>>(rows),
                InsulationDefaultColumns = _mapper.Map<IEnumerable<InsulationDefaultColumnResultDto>>(columns).OrderBy(c => c.MinOperatingTemperature)
            };

            var detailDtos = new List<InsulationDefaultDetailResultDto>();
            foreach (var r in rows)
            {
                foreach (var c in columns)
                {
                    var cellDetails = await _insulationDefaultDetailService
                        .GetByInsulationDefaultId(r.Id, c.Id);
                    detailDtos.AddRange(
                        _mapper.Map<IEnumerable<InsulationDefaultDetailResultDto>>(cellDetails)
                    );
                }
            }
            viewModel.InsulationDefaultDetails = detailDtos;
            if (type == "D")
            {
                viewModel.GridData = await GetInsulationDefaultGrid(id);
            }
            return PartialView("_InsulationDefaultDetails", viewModel);
            //var insulationDefault = await _insulationDefaultService.GetById(id); // Fetch insulation details
            ////var defaultRow = await _insulationDefaultRowService.GetAll();
            ////var defaultDetail = await _insulationDefaultDetailService.GetAll();
            ////var insulationThicknesses = await _insulationThicknessService.GetAll();

            //var insulationDefaultRows = await _insulationDefaultRowService.GetByInsulationDefaultId(id);
            //var insulationDefaultColumns = await _insulationDefaultColumnService.GetByInsulationDefaultId(id);
            //var insulationDefaultDetails = await _insulationDefaultDetailService.GetAll();


            ////var insulationThicknesses = await _insulationThicknessService.GetById(id); // Fetch related thicknesses

            //InsulationDefaultDetailsViewModel insulationDefaultDetailsViewModel = new InsulationDefaultDetailsViewModel();
            //insulationDefaultDetailsViewModel.InsulationDefault = _mapper.Map<InsulationDefaultResultDto>(insulationDefault);

            //if (type == "D")
            //{
            //    //var insulationDefaultRows = await _epProjectInsulationDefaultRowService.GetByInsulationDefaultId(id);
            //    //var insulationDefaultColumns = await _epProjectInsulationDefaultColumnService.GetByInsulationDefaultId(id);

            //    //var insulationDefaultRows = await _insulationDefaultRowService.GetByInsulationDefaultId(id);
            //    //var insulationDefaultColumns = await _insulationDefaultColumnService.GetByInsulationDefaultId(id);
            //    insulationDefaultDetailsViewModel.InsulationDefaultRows = _mapper.Map<IEnumerable<InsulationDefaultRowResultDto>>(insulationDefaultRows);
            //    insulationDefaultDetailsViewModel.InsulationDefaultColumns = _mapper.Map<IEnumerable<InsulationDefaultColumnResultDto>>(insulationDefaultColumns).OrderBy(m => m.MinOperatingTemperature);
            //    insulationDefaultDetailsViewModel.GridData = await GetInsulationDefaultGrid(id);
            //    insulationDefaultDetailsViewModel.InsulationDefault = _mapper.Map<InsulationDefaultResultDto>(insulationDefault);
            //}

            //if (insulationDefault == null)
            //{
            //    return NotFound();
            //}

            //return PartialView("_InsulationDefaultDetails", insulationDefaultDetailsViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailsEp(Guid id, string type = "D")
        {
            var insulationDefault = await _epProjectInsulationDefaultService.GetById(id); // Fetch insulation details
            var insulationThicknesses = await _insulationThicknessService.GetById(id); // Fetch related thicknesses

            EpProjectInsulationDefaultDetailsViewModel insulationDefaultDetailsViewModel = new EpProjectInsulationDefaultDetailsViewModel();
            insulationDefaultDetailsViewModel.InsulationDefault = _mapper.Map<EpProjectInsulationDefaultResultDto>(insulationDefault);

            if (type == "D")
            {
                var insulationDefaultRows = await _epProjectInsulationDefaultRowService.GetByInsulationDefaultId(id);
                var insulationDefaultColumns = await _epProjectInsulationDefaultColumnService.GetByInsulationDefaultId(id);
                insulationDefaultDetailsViewModel.InsulationDefaultRows = _mapper.Map<IEnumerable<EpProjectInsulationDefaultRowResultDto>>(insulationDefaultRows);
                insulationDefaultDetailsViewModel.InsulationDefaultColumns = _mapper.Map<IEnumerable<EpProjectInsulationDefaultColumnResultDto>>(insulationDefaultColumns).OrderBy(m => m.MinOperatingTemperature);
                insulationDefaultDetailsViewModel.GridData = await GetEpProjectInsulationDefaultGrid(id);
                insulationDefaultDetailsViewModel.InsulationDefault = _mapper.Map<EpProjectInsulationDefaultResultDto>(insulationDefault);
            }

            if (insulationDefault == null)
            {
                return NotFound();
            }

            return PartialView("_InsulationDefaultDetailsEp", insulationDefaultDetailsViewModel);
        }

        public async Task<List<InsulationDefaultGridViewModel>> GetInsulationDefaultGrid(Guid id)
        {
            var rows = _insulationDefaultRowService.GetByInsulationDefaultId(id).Result.OrderBy(m => m.SizeNps.SortOrder).OrderBy(m => m.SizeNps.SortOrder);
            var cols = _insulationDefaultColumnService.GetByInsulationDefaultId(id).Result.OrderBy(m => m.MinOperatingTemperature).OrderBy(m => m.MinOperatingTemperature);

            if (!rows.Any() || !cols.Any())
                return new List<InsulationDefaultGridViewModel>();

            var gridData = new List<InsulationDefaultGridViewModel>();

            foreach (var row in rows)
            {
                var rowData = new InsulationDefaultGridViewModel
                {
                    RowId = row.Id,
                    Nps = row.SizeNps?.Name ?? string.Empty
                };

                foreach (var col in cols)
                {
                    var temperatureRange = (col.MinOperatingTemperature ?? -273) == -273
                        ? "<=" + col.MaxOperatingTemperature + "°C"
                        : col.MinOperatingTemperature + " to " + col.MaxOperatingTemperature + "°C";

                    var details = _insulationDefaultDetailService.GetByInsulationDefaultId(row.Id, col.Id).Result.FirstOrDefault();

                    var columnData = new InsulationDefaultColumnViewModel
                    {
                        ColumnId = col.Id,
                        TemperatureRange = temperatureRange,
                        InsulationThickness = details?.InsulationThickness?.Name ?? "-",
                        NumberOfTracers = details?.TracingDesignNumberOfTracers?.Name ?? "-",
                        DetailsUrl = details != null
                            ? $"Id={details.Id}&InsulationDefaultId={id}"
                            : $"RowId={row.Id}&ColumnId={col.Id}&InsulationDefaultId={id}"
                    };

                    rowData.Columns.Add(columnData);
                }

                gridData.Add(rowData);
            }

            return gridData;
        }

        public async Task<List<EpProjectInsulationDefaultGridViewModel>> GetEpProjectInsulationDefaultGrid(Guid id)
        {
            var rows = _epProjectInsulationDefaultRowService.GetByInsulationDefaultId(id).Result.OrderBy(m => m.SizeNps.SortOrder).OrderBy(m => m.SizeNps.SortOrder);
            var cols = _epProjectInsulationDefaultColumnService.GetByInsulationDefaultId(id).Result.OrderBy(m => m.MinOperatingTemperature).OrderBy(m => m.MinOperatingTemperature);

            if (!rows.Any() || !cols.Any())
                return new List<EpProjectInsulationDefaultGridViewModel>();

            var gridData = new List<EpProjectInsulationDefaultGridViewModel>();

            foreach (var row in rows)
            {
                var rowData = new EpProjectInsulationDefaultGridViewModel
                {
                    RowId = row.Id,
                    Nps = row.SizeNps?.Name ?? string.Empty
                };

                foreach (var col in cols)
                {
                    var temperatureRange = (col.MinOperatingTemperature ?? -273) == -273
                        ? "<=" + col.MaxOperatingTemperature + "°C"
                        : col.MinOperatingTemperature + " to " + col.MaxOperatingTemperature + "°C";

                    var details = _epProjectInsulationDefaultDetailService.GetByInsulationDefaultId(row.Id, col.Id).Result.FirstOrDefault();

                    var columnData = new EpProjectInsulationDefaultColumnViewModel
                    {
                        ColumnId = col.Id,
                        TemperatureRange = temperatureRange,
                        InsulationThickness = details?.InsulationThickness?.Name ?? "-",
                        NumberOfTracers = details?.TracingDesignNumberOfTracers?.Name ?? "-",
                        DetailsUrl = details != null
                            ? $"Id={details.Id}&InsulationDefaultId={id}"
                            : $"RowId={row.Id}&ColumnId={col.Id}&InsulationDefaultId={id}"
                    };

                    rowData.Columns.Add(columnData);
                }

                gridData.Add(rowData);
            }

            return gridData;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var specifications = await _specificationService.GetAll();
            var specification = specifications.Where(m => m.Name == "TR").FirstOrDefault();
            var specificationName = specification?.Name_dash_Description;
            var model = new InsulationDefaultAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName,
                SpecificationName = specificationName,
                SpecificationId = specification?.Id
            };
            var mats = await _insulationMaterialService.GetAll();
            ViewBag.InsulationMaterials = mats
                .Where(m => m.IsActive)
                .OrderBy(m => m.SortOrder);

            var types = await _insulationTypeService.GetAll();
            ViewBag.InsulationTypes = types
                .Where(t => t.IsActive)
                .OrderBy(t => t.SortOrder);

            var tracers = await _tracingTypeService.GetAll();
            ViewBag.TracingTypes = tracers
                .Where(tr => tr.IsActive)
                .OrderBy(tr => tr.SortOrder);

            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(InsulationDefaultAddDto model)
        {
            if (!ModelState.IsValid)
            {
                // Log this
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, errorMessage = string.Join("; ", errors) });
            }

            var insulationDefault = _mapper.Map<InsulationDefault>(model);
            await _insulationDefaultService.Add(insulationDefault);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var specifications = await _specificationService.GetAll();
            var specification = specifications.Where(m => m.Name == "TR").FirstOrDefault();
            var specificationName = specification?.Name_dash_Description;
            var insulationDefault = await _insulationDefaultService.GetById(id);
            if (insulationDefault == null)
                return NotFound();

            var insulationDefaultDto = _mapper.Map<InsulationDefaultEditDto>(insulationDefault);
            insulationDefaultDto.SpecificationName = specificationName;

            var mats = await _insulationMaterialService.GetAll();
            ViewBag.InsulationMaterials = mats
                .Where(m => m.IsActive)
                .OrderBy(m => m.SortOrder);

            var types = await _insulationTypeService.GetAll();
            ViewBag.InsulationTypes = types
                .Where(t => t.IsActive)
                .OrderBy(t => t.SortOrder);

            var tracers = await _tracingTypeService.GetAll();
            ViewBag.TracingTypes = tracers
                .Where(tr => tr.IsActive)
                .OrderBy(tr => tr.SortOrder);

            return PartialView("_Update", insulationDefaultDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InsulationDefaultEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var insulationDefault = _mapper.Map<InsulationDefault>(model);
            await _insulationDefaultService.Update(insulationDefault);

            return Json(new { success = true });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> UpdateEP(Guid id)
        {
            var specifications = await _specificationService.GetAll();
            var specification = specifications.Where(m => m.Name == "TR").FirstOrDefault();
            var specificationName = specification?.Name_dash_Description;
            var insulationDefault = await _epProjectInsulationDefaultService.GetById(id);
            if (insulationDefault == null)
                return NotFound();

            var insulationDefaultDto = _mapper.Map<EpProjectInsulationDefaultEditDto>(insulationDefault);
            insulationDefaultDto.SpecificationName = specificationName;

            ViewBag.InsulationMaterials = _insulationMaterialService.GetAll().Result.Where(r=>r.IsActive).OrderBy(r=>r.SortOrder).ToList();
            ViewBag.InsulationTypes =  _insulationTypeService.GetAll().Result.Where(r => r.IsActive).OrderBy(r => r.SortOrder).ToList();
            ViewBag.TracingTypes =  _tracingTypeService.GetAll().Result.Where(r => r.IsActive).OrderBy(r => r.SortOrder).ToList();

            return PartialView("_UpdateEP", insulationDefaultDto);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> UpdateEP(EpProjectInsulationDefaultEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var insulationDefault = _mapper.Map<EpProjectInsulationDefault>(model);
            await _epProjectInsulationDefaultService.Update(insulationDefault);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var insulationDefault = await _insulationDefaultService.GetById(id);
            if (insulationDefault == null)
                return Json(new { success = false, ErrorMessage = "Insulation default not found" });

            var hasRows = (await _insulationDefaultRowService.GetByInsulationDefaultId(id)).Any();
            var hasCols = (await _insulationDefaultColumnService.GetByInsulationDefaultId(id)).Any();
            if (hasRows || hasCols)
            {
                return Json(new
                {
                    success = false,
                    ErrorMessage = "Cannot delete: this table default has existing row/column definitions."
                });
            }

            await _insulationDefaultService.Remove(insulationDefault);
            return Json(new { success = true });
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<JsonResult> DeleteEp(Guid id)
        {
            var insulationDefault = await _epProjectInsulationDefaultService.GetById(id);
            if (insulationDefault == null)
                return Json(new { success = false, ErrorMessage = "Insulation default not found" });


            var details = await _epProjectInsulationDefaultDetailService.GetByInsulationDefaultId(id);
            foreach (var item in details)
                await _epProjectInsulationDefaultDetailService.Remove(item);

            var rows = await _epProjectInsulationDefaultRowService.GetByInsulationDefaultId(id);
            foreach (var item in rows)
                await _epProjectInsulationDefaultRowService.Remove(item);

            var cols = await _epProjectInsulationDefaultColumnService.GetByInsulationDefaultId(id);
            foreach (var item in cols)
                await _epProjectInsulationDefaultColumnService.Remove(item);

            await _epProjectInsulationDefaultService.Remove(insulationDefault);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentInsulationDefault = await _insulationDefaultService.GetById(request.Id);
            if (currentInsulationDefault == null)
                return Json(new { success = false, ErrorMessage = "Insulation default not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            var swapInsulationDefault = (await _insulationDefaultService.GetAll())
                .Where(i => isMoveUp ? i.SortOrder < currentInsulationDefault.SortOrder : i.SortOrder > currentInsulationDefault.SortOrder)
                .OrderBy(i => isMoveUp ? i.SortOrder * -1 : i.SortOrder)
                .FirstOrDefault();

            if (swapInsulationDefault == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No insulation default to move up." : "No insulation default to move down." });

            int tempSortOrder = currentInsulationDefault.SortOrder;
            currentInsulationDefault.SortOrder = swapInsulationDefault.SortOrder;
            swapInsulationDefault.SortOrder = tempSortOrder;

            await _insulationDefaultService.Update(currentInsulationDefault);
            await _insulationDefaultService.Update(swapInsulationDefault);

            return Json(new { success = true });
        }
    }
}
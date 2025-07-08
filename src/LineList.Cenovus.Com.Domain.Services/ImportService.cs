using LineList.Cenovus.Com.API.DataTransferObjects.Import;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportSheet;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
//using LineList.Cenovus.Com.RulesEngine; 
//using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportService : IImportService
    {
        private readonly IImportRepository _importRepository;
        private readonly ILogger<ImportService> _logger;

        // Additional dependencies for extended functionality (commented out)
        /*
        private readonly ILineRevisionService _lineRevisionService;
        private readonly ILineListService _lineListService;
        private readonly IFacilityService _facilityService;
        private readonly ICommodityService _commodityService;
        private readonly ILocationService _locationService;
        private readonly ISizeNpsService _sizeNpsService;
        private readonly IInsulationThicknessService _insulationThicknessService;
        private readonly ITracingTypeService _tracingTypeService;
        private readonly IInsulationTypeService _insulationTypeService;
        private readonly IPipeSpecificationService _pipeSpecificationService;
        private readonly IFlatFactoryService _flatFactoryService;
        private readonly CurrentUser _currentUser;
        private readonly Validator _validator;
        */

        private const string DocumentNumberList = "DOCUMENT NUMBER LIST";
        private const string LdtReportTemplate = "LDT REPORT TEMPLATE";
        private const string DateFormat = "yyyy-MM-dd";
        private const int HeaderRowLDTLineLists = 0;
        private const int HeaderRowLDTLines = 0;

        public ImportService(
            IImportRepository importRepository,
            ILogger<ImportService> logger
        /* Additional parameters for extended functionality
        ILineRevisionService lineRevisionService,
        ILineListService lineListService,
        IFacilityService facilityService,
        ICommodityService commodityService,
        ILocationService locationService,
        ISizeNpsService sizeNpsService,
        IInsulationThicknessService insulationThicknessService,
        ITracingTypeService tracingTypeService,
        IInsulationTypeService insulationTypeService,
        IPipeSpecificationService pipeSpecificationService,
        IFlatFactoryService flatFactoryService,
        CurrentUser currentUser
        */
        )
        {
            _importRepository = importRepository;
            _logger = logger;
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Uncommented as it’s required for EPPlus

            // Initialize additional dependencies (commented out)
            /*
            _lineRevisionService = lineRevisionService;
            _lineListService = lineListService;
            _facilityService = facilityService;
            _commodityService = commodityService;
            _locationService = locationService;
            _sizeNpsService = sizeNpsService;
            _insulationThicknessService = insulationThicknessService;
            _tracingTypeService = tracingTypeService;
            _insulationTypeService = insulationTypeService;
            _pipeSpecificationService = pipeSpecificationService;
            _flatFactoryService = flatFactoryService;
            _currentUser = currentUser;
            _validator = new Validator();
            */
        }

        // CRUD methods
        public async Task<IEnumerable<Import>> GetAll()
        {
            _logger.LogInformation("Fetching all imports.");
            var imports = await _importRepository.GetAll();
            return imports ?? new List<Import>();
        }

        public async Task<Import> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching import with ID: {id}.");
            return await _importRepository.GetById(id);
        }

        public async Task<Import> Add(Import import)
        {
            _logger.LogInformation($"Adding new import with FacilityName: {import.FacilityName}.");
            if ((await _importRepository.Search(c => c.FacilityName == import.FacilityName)).Any())
            {
                _logger.LogWarning($"Duplicate FacilityName found: {import.FacilityName}.");
                return null;
            }

            await _importRepository.Add(import);
            await _importRepository.SaveChanges();
            return import;
        }

        public async Task<Import> Update(Import import)
        {
            _logger.LogInformation($"Updating import with ID: {import.Id}.");
            if ((await _importRepository.Search(c => c.FacilityName == import.FacilityName && c.Id != import.Id)).Any())
            {
                _logger.LogWarning($"Duplicate FacilityName found during update: {import.FacilityName}.");
                return null;
            }

            await _importRepository.Update(import);
            await _importRepository.SaveChanges();
            return import;
        }

        public async Task<bool> Remove(Import import)
        {
            _logger.LogInformation($"Removing import with ID: {import.Id}.");
            await _importRepository.Remove(import);
            await _importRepository.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<Import>> Search(string searchCriteria)
        {
            _logger.LogInformation($"Searching imports with criteria: {searchCriteria}.");
            return await _importRepository.Search(c => c.FacilityName.Contains(searchCriteria));
        }

        // LDT Import Module-specific methods
        public async Task<string> ValidateBeforeUpload(IFormFile file)
        {
            _logger.LogInformation($"Validating Excel file: {file.FileName}.");

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);

                var worksheets = package.Workbook.Worksheets;
                var lineListSheet = worksheets.FirstOrDefault(ws => ws.Name.Contains(DocumentNumberList));
                var lineSheet = worksheets.FirstOrDefault(ws => ws.Name.Contains(LdtReportTemplate));

                if (lineListSheet == null || lineListSheet.Dimension?.Columns != 8 ||
                    lineSheet == null || lineSheet.Dimension?.Columns != 52)
                {
                    _logger.LogWarning("Invalid template: Sheet names or column counts do not match.");
                    return "Valid template sheet / Column mismatching.";
                }

                bool docLineSheetMatch = true;
                for (int i = 2; i <= lineSheet.Dimension.Rows; i++)
                {
                    string lineDocNo = lineSheet.Cells[i, 51].Text; // Column 51 (0-based) = excelDocNo
                    bool isExist = false;
                    for (int j = 2; j <= lineListSheet.Dimension.Rows; j++)
                    {
                        string lineListDocNo = lineListSheet.Cells[j, 2].Text; // Column 1 (0-based) = excelDocumentNumber
                        if (lineDocNo == lineListDocNo)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist && !string.IsNullOrWhiteSpace(lineDocNo))
                    {
                        docLineSheetMatch = false;
                        break;
                    }
                }

                bool docLineListSheetMatch = true;
                for (int i = 2; i <= lineListSheet.Dimension.Rows; i++)
                {
                    string lineListDocNo = lineListSheet.Cells[i, 2].Text;
                    bool isExist = false;
                    for (int j = 2; j <= lineSheet.Dimension.Rows; j++)
                    {
                        string lineDocNo = lineSheet.Cells[j, 51].Text;
                        if (lineListDocNo == lineDocNo)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist && !string.IsNullOrWhiteSpace(lineListDocNo))
                    {
                        docLineListSheetMatch = false;
                        break;
                    }
                }

                if (!docLineListSheetMatch || !docLineSheetMatch)
                {
                    _logger.LogWarning("Document numbers mismatch between Line and LineList sheets.");
                    return "DocumentNumber(s) are mismatching between Line & LineList sheet.";
                }

                _logger.LogInformation("File structure validation successful.");
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during validation of file: {file.FileName}.");
                return $"Validation error: {ex.Message}";
            }
        }

        public async Task<Import> Import(IFormFile file, string userName)
        {
            _logger.LogInformation($"Processing Excel file: {file.FileName} by user: {userName}.");

            var import = new Import
            {
                Id = Guid.NewGuid(),
                OriginalFileName = file.FileName,
                FacilityName = ExtractFacilityName(file.FileName),
                Status = "Pending",
                Path = string.Empty, // Path should be set by the controller if needed
                CreatedOn = DateTime.UtcNow,
                CreatedBy = userName,
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = userName,
                ValidationCount = 0
            };

            await _importRepository.AddImport(import);
            await _importRepository.SaveChanges();

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);

                // Validate sheets
                import.Status = "ValidationRequested";
                await _importRepository.UpdateImport(import);
                await _importRepository.SaveChanges();

                var linesSheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == LdtReportTemplate);
                var lineListsSheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == DocumentNumberList);

                if (linesSheet == null || lineListsSheet == null)
                {
                    throw new Exception("Required sheets not found: 'LDT REPORT TEMPLATE' and 'DOCUMENT NUMBER LIST' must be present.");
                }

                if (linesSheet.Dimension?.Columns != 52)
                {
                    throw new Exception($"LDT REPORT TEMPLATE sheet must have exactly 52 columns, but found {linesSheet.Dimension?.Columns}.");
                }

                if (lineListsSheet.Dimension?.Columns != 8)
                {
                    throw new Exception($"DOCUMENT NUMBER LIST sheet must have exactly 8 columns, but found {lineListsSheet.Dimension?.Columns}.");
                }

                // Fix header rows
                LDTLineFixHeaderRow(linesSheet);
                LDTLineListFixHeaderRow(lineListsSheet);

                // Commented out: Metrics creation
                // await CreateMetricsAsync(import);

                // Process Lines Sheet
                var linesSheetModel = new ImportSheet
                {
                    Id = Guid.NewGuid(),
                    ImportId = import.Id,
                    Name = LdtReportTemplate,
                    SheetType = "Lines",
                    NumberOfRows = linesSheet.Dimension.Rows - 1,
                    NumberOfColumns = 52,
                    NumberOfAccepted = 0,
                    NumberOfImported = 0,
                    NumberOfExceptions = 0
                };
                await _importRepository.AddSheet(linesSheetModel);
                await _importRepository.SaveChanges();

                var lineExceptions = new List<ImportRowException>();
                var lineRows = new List<ImportRow>();
                for (int row = 2; row <= linesSheet.Dimension.Rows; row++)
                {
                    if (IsRowBlank(linesSheet, row, 52))
                        continue;

                    var importRow = new ImportRow
                    {
                        Id = Guid.NewGuid(),
                        ImportSheetId = linesSheetModel.Id,
                        RowNumber = row,
                        IsAccepted = false,
                        IsImported = false
                    };

                    for (int col = 1; col <= 52; col++)
                    {
                        var columnName = linesSheet.Cells[HeaderRowLDTLines + 1, col].Text;
                        if (string.IsNullOrWhiteSpace(columnName))
                            continue;

                        var value = linesSheet.Cells[row, col].Text;
                        if (columnName.StartsWith("excel") && columnName.Contains("Date") && DateTime.TryParse(value, out var date))
                        {
                            value = date.ToString(DateFormat);
                        }
                        importRow[columnName] = value;
                    }

                    if (string.IsNullOrWhiteSpace(importRow.excelLineNo))
                    {
                        lineExceptions.Add(new ImportRowException
                        {
                            Id = Guid.NewGuid(),
                            ImportRowId = importRow.Id,
                            Message = "Line Number is required."
                        });
                        continue;
                    }

                    if (await _importRepository.LineExists(importRow.excelLineNo))
                    {
                        lineExceptions.Add(new ImportRowException
                        {
                            Id = Guid.NewGuid(),
                            ImportRowId = importRow.Id,
                            Message = $"Line Number '{importRow.excelLineNo}' already exists."
                        });
                        continue;
                    }

                    importRow.IsAccepted = true;
                    lineRows.Add(importRow);
                    linesSheetModel.NumberOfAccepted++;
                }

                // Process Line Lists Sheet
                var lineListsSheetModel = new ImportSheet
                {
                    Id = Guid.NewGuid(),
                    ImportId = import.Id,
                    Name = DocumentNumberList,
                    SheetType = "LineLists",
                    NumberOfRows = lineListsSheet.Dimension.Rows - 1,
                    NumberOfColumns = 8,
                    NumberOfAccepted = 0,
                    NumberOfImported = 0,
                    NumberOfExceptions = 0
                };
                await _importRepository.AddSheet(lineListsSheetModel);
                await _importRepository.SaveChanges();

                var lineListExceptions = new List<ImportRowException>();
                var lineListRows = new List<ImportRow>();
                for (int row = 2; row <= lineListsSheet.Dimension.Rows; row++)
                {
                    if (IsRowBlank(lineListsSheet, row, 8))
                        continue;

                    var importRow = new ImportRow
                    {
                        Id = Guid.NewGuid(),
                        ImportSheetId = lineListsSheetModel.Id,
                        RowNumber = row,
                        IsAccepted = false,
                        IsImported = false
                    };

                    for (int col = 1; col <= 8; col++)
                    {
                        var columnName = lineListsSheet.Cells[HeaderRowLDTLineLists + 1, col].Text;
                        if (string.IsNullOrWhiteSpace(columnName))
                            continue;

                        var value = lineListsSheet.Cells[row, col].Text;
                        if (columnName.StartsWith("excel") && columnName.Contains("Date") && DateTime.TryParse(value, out var date))
                        {
                            value = date.ToString(DateFormat);
                        }
                        importRow[columnName] = value;
                    }

                    if (string.IsNullOrWhiteSpace(importRow.excelDocumentNumber))
                    {
                        lineListExceptions.Add(new ImportRowException
                        {
                            Id = Guid.NewGuid(),
                            ImportRowId = importRow.Id,
                            Message = "Document Number is required."
                        });
                        continue;
                    }

                    if (await _importRepository.LineListExists(importRow.excelDocumentNumber))
                    {
                        lineListExceptions.Add(new ImportRowException
                        {
                            Id = Guid.NewGuid(),
                            ImportRowId = importRow.Id,
                            Message = $"Document Number '{importRow.excelDocumentNumber}' already exists."
                        });
                        continue;
                    }

                    importRow.IsAccepted = true;
                    lineListRows.Add(importRow);
                    lineListsSheetModel.NumberOfAccepted++;
                }

                // Commented out: Additional validation using Validator
                /*
                var flatLists = _flatFactoryService.ToFlatLineLists(lineListRows);
                var flatLines = await _flatFactoryService.ToFlatLinesAsync(lineRows);

                lineListExceptions.AddRange(_validator.Validate(flatLists).Select(e => new ImportRowException
                {
                    Id = Guid.NewGuid(),
                    ImportRowId = e.LineList.Id,
                    Message = e.Validation.Message
                }));

                lineExceptions.AddRange(_validator.Validate(flatLines).Select(e => new ImportRowException
                {
                    Id = Guid.NewGuid(),
                    ImportRowId = e.Line.Id,
                    Message = e.Validation.Message
                }));
                */

                // Save rows individually since AddRows is not available
                foreach (var row in lineRows.Concat(lineListRows))
                {
                    await _importRepository.AddRow(row);
                }
                await _importRepository.SaveChanges();

                // Update exception counts
                foreach (var exception in lineExceptions.Concat(lineListExceptions))
                {
                    await _importRepository.AddException(exception);
                }
                await _importRepository.SaveChanges();

                linesSheetModel.NumberOfExceptions = lineExceptions.Count;
                lineListsSheetModel.NumberOfExceptions = lineListExceptions.Count;

                // Update import status
                import.Status = (lineExceptions.Count + lineListExceptions.Count) > 0 ? "ValidationComplete" : "ImportRequested";
                import.ValidationCount = linesSheetModel.NumberOfExceptions + lineListsSheetModel.NumberOfExceptions;

                await _importRepository.UpdateImport(import);
                await _importRepository.SaveChanges();

                // Commented out: Process import into Line and LineRevision entities
                /*
                if (import.Status == "ImportRequested")
                {
                    await ProcessImportAsync(import, lineListRows, lineRows, userName);
                }

                await UpdateMetricsAsync(import.Id);
                */

                _logger.LogInformation($"Import completed for file: {file.FileName}. Status: {import.Status}.");
                return import;
            }
            catch (Exception ex)
            {
                import.Status = "Error";
                await _importRepository.UpdateImport(import);
                await _importRepository.AddException(new ImportRowException
                {
                    Id = Guid.NewGuid(),
                    ImportRowId = Guid.Empty,
                    Message = ex.Message
                });
                await _importRepository.SaveChanges();
                _logger.LogError(ex, $"Error during import of file: {file.FileName}.");
                throw;
            }
        }

        public async Task<ImportResultDto> GetImportDetails(Guid id)
        {
            _logger.LogInformation($"Fetching import details for ID: {id}.");
            return await _importRepository.GetWithDetails(id);
        }

        public async Task<ImportSheetResultDto> GetSheetDetails(Guid importSheetId)
        {
            return await _importRepository.GetSheetWithDetails(importSheetId);
        }

        // Commented out: Metrics tracking methods
        /*
        private async Task CreateMetricsAsync(Import import)
        {
            var commodities = await _commodityService.GetAll();
            var facilities = await _facilityService.GetAll();
            var locations = await _locationService.GetAll();

            foreach (var facility in facilities)
            {
                var count = await _lineRevisionService.GetCountByFacilityId(facility.Id);
                await _importRepository.AddFacility(new ImportFacility
                {
                    Id = Guid.NewGuid(),
                    ImportId = import.Id,
                    FacilityId = facility.Id,
                    BeforeCount = count,
                    AfterCount = 0
                });
            }

            foreach (var commodity in commodities)
            {
                var count = await _lineRevisionService.GetCountByCommodityId(commodity.Id);
                await _importRepository.AddCommodity(new ImportCommodity
                {
                    Id = Guid.NewGuid(),
                    ImportId = import.Id,
                    CommodityId = commodity.Id,
                    BeforeCount = count,
                    AfterCount = 0
                });
            }

            foreach (var location in locations)
            {
                var count = await _lineRevisionService.GetCountByLocationId(location.Id);
                await _importRepository.AddLocation(new ImportLocation
                {
                    Id = Guid.NewGuid(),
                    ImportId = import.Id,
                    LocationId = location.Id,
                    BeforeCount = count,
                    AfterCount = 0
                });
            }
            await _importRepository.SaveChanges();
        }

        private async Task UpdateMetricsAsync(Guid importId)
        {
            var import = await _importRepository.GetById(importId);
            if (import == null) return;

            var commodities = await _importRepository.GetCommodities(importId);
            foreach (var commodity in commodities)
            {
                commodity.AfterCount = await _lineRevisionService.GetCountByCommodityId(commodity.CommodityId);
                await _importRepository.UpdateCommodity(commodity);
            }

            var facilities = await _importRepository.GetFacilities(importId);
            foreach (var facility in facilities)
            {
                facility.AfterCount = await _lineRevisionService.GetCountByFacilityId(facility.FacilityId);
                await _importRepository.UpdateFacility(facility);
            }

            var locations = await _importRepository.GetLocations(importId);
            foreach (var location in locations)
            {
                location.AfterCount = await _lineRevisionService.GetCountByLocationId(location.LocationId);
                await _importRepository.UpdateLocation(location);
            }
            await _importRepository.SaveChanges();
        }
        */

        // Commented out: Process import into domain entities
        /*
        private async Task ProcessImportAsync(Import import, IEnumerable<ImportRow> lineListRows, IEnumerable<ImportRow> lineRows, string userName)
        {
            var lineListFlat = _flatFactoryService.ToFlatLineLists(lineListRows);
            var lineListRevisions = _flatFactoryService.ToLineListRevisions(lineListFlat, userName);
            var lineListIds = lineListRevisions.ToDictionary(lr => lr.LineList.DocumentNumber, lr => lr.Id);

            var facility = import.FacilityName;
            var lineFlat = await _flatFactoryService.ToFlatLinesAsync(lineRows);
            var lineRevisions = _flatFactoryService.ToLineRevisions(lineFlat, userName, facility, false, false, lineListIds).ToList();

            foreach (var row in lineListRows.Concat(lineRows))
            {
                row.IsImported = true;
                await _importRepository.UpdateRow(row);
            }
            await _importRepository.SaveChanges();

            foreach (var item in lineRevisions)
            {
                item.LineNumber = LineNumberGenerator.Evaluate(item, _locationService, _commodityService, _pipeSpecificationService, _sizeNpsService, _insulationThicknessService, _tracingTypeService, _insulationTypeService);
                item.ValidationState = (int)LineRevisionValidation.GetHardRevisionValidationState(item);
            }

            import.Status = "ImportComplete";
            await _importRepository.UpdateImport(import);
            await _importRepository.SaveChanges();
        }
        */

        // Helper methods
        private string ExtractFacilityName(string fileName)
        {
            if (fileName.Contains("Import Template"))
            {
                var segments = fileName.Split('-');
                return segments[segments.Length - 1].Split('.')[0].Trim();
            }
            return fileName.Contains("-") ? fileName.Substring(0, fileName.IndexOf("-")) :
                fileName.Substring(0, 2).ToUpperInvariant() == "CL" ? "CL" : "FC";
        }

        private bool IsRowBlank(ExcelWorksheet worksheet, int row, int columnCount)
        {
            for (int col = 1; col <= columnCount; col++)
            {
                if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                    return false;
            }
            return true;
        }

        private void LDTLineFixHeaderRow(ExcelWorksheet worksheet)
        {
            string[] columnNames = new[]
            {
                "excelAltOpMode", "excelParentChild", "excelAreaID", "excelSpec", "excelLocationID", "excelCommCode",
                "excelClassServMat", "excelSizeNPS", "excelLineNo", "excelInsulThk", "excelInsulType",
                "excelTraceDesignType", "excelTraceDesignHoldTemp", "excelTracingDesignNumTracers", "excelInsMat",
                "excelLineFrom", "excelLineTo", "excelOrigPIDNo", "excelSched", "excelThk", "excelFluidPhase",
                "excelOpPress", "excelOpTemp", "excelDesignPress", "excelDesignMaxTemp", "excelDesignMinTemp",
                "excelTestPress", "excelTestMed", "excelExpTemp", "excelUpsetPress", "excelUpsetTemp", "excelMDMTTemp",
                "excelCorrAllow", "excelXray", "excelNDECat", "excelPWHT", "excelStressRel", "excelPaintSys",
                "excelIntCoatLiner", "excelCode", "excelABSARegistration", "excelPressureProtection", "excelAsBuilt",
                "excelFluid", "excelCsaClassLocation", "excelCSALVPHVP", "excelPipeMaterialSpecifications",
                "excelHoopStressLevel", "excelSourService", "excelNotes", "excelLineRev", "excelDocNo"
            };

            for (int col = 1; col <= worksheet.Dimension.Columns && col <= columnNames.Length; col++)
            {
                worksheet.Cells[HeaderRowLDTLines + 1, col].Value = columnNames[col - 1];
            }
        }

        private void LDTLineListFixHeaderRow(ExcelWorksheet worksheet)
        {
            string[] columnNames = new[]
            {
                "excelRev", "excelDocumentNumber", "excelStatus", "excelEP", "excelEPProj", "excelSpec",
                "excelDateIssued", "excelDescription"
            };

            for (int col = 1; col <= worksheet.Dimension.Columns && col <= columnNames.Length; col++)
            {
                worksheet.Cells[HeaderRowLDTLineLists + 1, col].Value = columnNames[col - 1];
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing ImportService.");
            _importRepository?.Dispose();

            // Commented out: Dispose additional services
            /*
            _lineRevisionService?.Dispose();
            _lineListService?.Dispose();
            _facilityService?.Dispose();
            _commodityService?.Dispose();
            _locationService?.Dispose();
            _sizeNpsService?.Dispose();
            _insulationThicknessService?.Dispose();
            _tracingTypeService?.Dispose();
            _insulationTypeService?.Dispose();
            _pipeSpecificationService?.Dispose();
            */
        }
    }
}
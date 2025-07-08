using LineList.Cenovus.Com.API.DataTransferObjects;
using LineList.Cenovus.Com.API.DataTransferObjects.Import;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportCommodity;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportFacility;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportLocation;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportRow;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportRowException;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportSheet;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ImportRepository : Repository<Import>, IImportRepository
    {
        private readonly LineListDbContext _context;

        public ImportRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Import>> GetAll()
        {
            return await _context.Imports.ToListAsync();
        }

        public async Task<Import> GetById(Guid id)
        {
            return await _context.Imports.FindAsync(id);
        }

        public async Task<ImportResultDto> GetWithDetails(Guid id)
        {
            return await _context.Imports
                .Where(i => i.Id == id)
                .Select(i => new ImportResultDto
                {
                    Id = i.Id,
                    OriginalFileName = i.OriginalFileName,
                    FacilityName = i.FacilityName,
                    Status = i.Status,
                    Path = i.Path,
                    ValidationCount = i.ValidationCount,
                    CreatedBy = i.CreatedBy,
                    CreatedOn = i.CreatedOn,
                    ModifiedBy = i.ModifiedBy,
                    ModifiedOn = i.ModifiedOn,
                    ImportSheets = i.ImportSheets.Select(s => new ImportSheetResultDto
                    {
                        Id = s.Id,
                        ImportId = s.ImportId,
                        Name = s.Name,
                        SheetType = s.SheetType,
                        NumberOfRows = s.NumberOfRows,
                        NumberOfAccepted = s.NumberOfAccepted,
                        NumberOfImported = s.NumberOfImported,
                        NumberOfExceptions = s.NumberOfExceptions,
                        IgnoredFields = s.IgnoredFields
                    }).ToList(),
                    ImportCommodities = i.ImportCommodities.Select(c => new ImportCommodityResultDto
                    {
                        Id = c.Id,
                        ImportId = c.ImportId,
                        CommodityId = c.CommodityId,
                        BeforeCount = c.BeforeCount,
                        AfterCount = c.AfterCount,
                        Commodity = new CommodityResultDto
                        {
                            Id = c.Commodity.Id,
                            Name = c.Commodity.Name,
                            Specification = c.Commodity.Specification != null
                                ? new SpecificationResultDto
                                {
                                    Id = c.Commodity.Specification.Id,
                                    Name = c.Commodity.Specification.Name
                                }
                                : null
                        }
                    }).ToList(),
                    ImportFacilities = i.ImportFacilities.Select(f => new ImportFacilityResultDto
                    {
                        Id = f.Id,
                        ImportId = f.ImportId,
                        FacilityId = f.FacilityId,
                        BeforeCount = f.BeforeCount,
                        AfterCount = f.AfterCount,
                        Facility = new FacilityResultDto
                        {
                            Id = f.Facility.Id,
                            Name = f.Facility.Name
                        }
                    }).ToList(),
                    ImportLocations = i.ImportLocations.Select(l => new ImportLocationResultDto
                    {
                        Id = l.Id,
                        ImportId = l.ImportId,
                        LocationId = l.LocationId,
                        BeforeCount = l.BeforeCount,
                        AfterCount = l.AfterCount,
                        Location = new LocationResultDto
                        {
                            Id = l.Location.Id,
                            Name = l.Location.Name
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<ImportSheetResultDto> GetSheetWithDetails(Guid importSheetId)
        {
            return await _context.ImportSheets
                .Where(s => s.Id == importSheetId)
                .Select(s => new ImportSheetResultDto
                {
                    Id = s.Id,
                    ImportId = s.ImportId,
                    Name = s.Name,
                    SheetType = s.SheetType,
                    NumberOfRows = s.NumberOfRows,
                    NumberOfAccepted = s.NumberOfAccepted,
                    NumberOfImported = s.NumberOfImported,
                    NumberOfExceptions = s.NumberOfExceptions,
                    NumberOfColumns = s.NumberOfColumns,
                    IgnoredFields = s.IgnoredFields,
                    ImportRows = s.ImportRows
                        .Where(r => !r.IsAccepted) // Filter to show only rows with exceptions
                        .Select(r => new ImportRowResultDto
                        {
                            Id = r.Id,
                            ImportSheetId = r.ImportSheetId,
                            RowNumber = r.RowNumber,
                            IsAccepted = r.IsAccepted,
                            IsImported = r.IsImported,
                            excelABSARegistration = r.excelABSARegistration,
                            excelAltOpMode = r.excelAltOpMode,
                            excelAnalysis = r.excelAnalysis,
                            excelAreaID = r.excelAreaID,
                            excelAsBuilt = r.excelAsBuilt,
                            excelChecked = r.excelChecked,
                            excelClassServMat = r.excelClassServMat,
                            excelCode = r.excelCode,
                            excelCommCode = r.excelCommCode,
                            excelCommodity = r.excelCommodity,
                            excelCorrAllow = r.excelCorrAllow,
                            excelCsaClassLocation = r.excelCsaClassLocation,
                            excelCSALVPHVP = r.excelCSALVPHVP,
                            excelDateYMD = r.excelDateYMD,
                            excelDateIssued = r.excelDateIssued,
                            excelDeleted = r.excelDeleted,
                            excelDescription = r.excelDescription,
                            excelDesignMaxTemp = r.excelDesignMaxTemp,
                            excelDesignMinTemp = r.excelDesignMinTemp,
                            excelDesignPress = r.excelDesignPress,
                            excelDocNo = r.excelDocNo,
                            excelDocumentNumber = r.excelDocumentNumber,
                            excelDocRev = r.excelDocRev,
                            excelDuplicate = r.excelDuplicate,
                            excelEP = r.excelEP,
                            excelEPProj = r.excelEPProj,
                            excelExpTemp = r.excelExpTemp,
                            excelFileName = r.excelFileName,
                            excelFluid = r.excelFluid,
                            excelFluidPhase = r.excelFluidPhase,
                            excelHoopStressLevel = r.excelHoopStressLevel,
                            excelInsMat = r.excelInsMat,
                            excelInsulThk = r.excelInsulThk,
                            excelInsulType = r.excelInsulType,
                            excelIntCoatLiner = r.excelIntCoatLiner,
                            excelLineFrom = r.excelLineFrom,
                            excelLineNo = r.excelLineNo,
                            excelLineRev = r.excelLineRev,
                            excelLineTo = r.excelLineTo,
                            excelLocationID = r.excelLocationID,
                            excelMDMTTemp = r.excelMDMTTemp,
                            excelNDECat = r.excelNDECat,
                            excelNotes = r.excelNotes,
                            excelOpPress = r.excelOpPress,
                            excelOpTemp = r.excelOpTemp,
                            excelOrigPIDNo = r.excelOrigPIDNo,
                            excelPaintSys = r.excelPaintSys,
                            excelParentChild = r.excelParentChild,
                            excelPipeMaterial = r.excelPipeMaterial,
                            excelPipeMaterialSpecifications = r.excelPipeMaterialSpecifications,
                            excelPressureProtection = r.excelPressureProtection,
                            excelProjNo = r.excelProjNo,
                            excelPWHT = r.excelPWHT,
                            excelReservedBy = r.excelReservedBy,
                            excelRev = r.excelRev,
                            excelSched = r.excelSched,
                            excelSizeNPS = r.excelSizeNPS,
                            excelSourService = r.excelSourService,
                            excelSpec = r.excelSpec,
                            excelSpecification = r.excelSpecification,
                            excelStatus = r.excelStatus,
                            excelStressRel = r.excelStressRel,
                            excelTestMed = r.excelTestMed,
                            excelTestPress = r.excelTestPress,
                            excelThk = r.excelThk,
                            excelTraceDesignType = r.excelTraceDesignType,
                            excelTraceDesignHoldTemp = r.excelTraceDesignHoldTemp,
                            excelTracingDesignNumTracers = r.excelTracingDesignNumTracers,
                            excelUpsetPress = r.excelUpsetPress,
                            excelUpsetTemp = r.excelUpsetTemp,
                            excelWallThk = r.excelWallThk,
                            excelXray = r.excelXray,
                            excelModularId = r.excelModularId,
                            Exceptions = r.Exceptions.Select(e => new ImportRowExceptionResultDto
                            {
                                Id = e.Id,
                                ImportRowId = e.ImportRowId,
                                Message = e.Message
                            }).ToList()
                        }).ToList(),
                    Columns = s.ImportSheetColumns.Select(c => new ImportSheetColumnDto
                    {
                        NameInDatabase = c.NameInDatabase,
                        NameInExcel = c.NameInExcel,
                        SortOrder = c.SortOrder
                    }).OrderBy(c => c.SortOrder).ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> LineExists(string lineNumber)
        {
            if (string.IsNullOrWhiteSpace(lineNumber))
                return false;
            return await _context.LineRevisions
                .AnyAsync(lr => lr.LineNumber == lineNumber);
        }

        public async Task<bool> LineListExists(string documentNumber)
        {
            return await _context.LineLists.AnyAsync(ll => ll.DocumentNumber == documentNumber);
        }

        public async Task AddImport(Import import)
        {
            await _context.Imports.AddAsync(import);
        }

        public async Task UpdateImport(Import import)
        {
            _context.Imports.Update(import);
        }

        public async Task AddSheet(ImportSheet sheet)
        {
            await _context.ImportSheets.AddAsync(sheet);
        }

        public async Task AddRow(ImportRow row)
        {
            await _context.ImportRows.AddAsync(row);
        }

        public async Task UpdateRow(ImportRow row)
        {
            _context.ImportRows.Update(row);
        }

        public async Task AddException(ImportRowException exception)
        {
            await _context.ImportRowExceptions.AddAsync(exception);
        }

        public async Task AddFacility(ImportFacility facility)
        {
            await _context.ImportFacilities.AddAsync(facility);
        }

        public async Task AddCommodity(ImportCommodity commodity)
        {
            await _context.ImportCommodities.AddAsync(commodity);
        }

        public async Task AddLocation(ImportLocation location)
        {
            await _context.ImportLocations.AddAsync(location);
        }

        public async Task<List<ImportCommodity>> GetCommodities(Guid importId)
        {
            return await _context.ImportCommodities
                .Where(c => c.ImportId == importId)
                .ToListAsync();
        }

        public async Task<List<ImportFacility>> GetFacilities(Guid importId)
        {
            return await _context.ImportFacilities
                .Where(f => f.ImportId == importId)
                .ToListAsync();
        }

        public async Task<List<ImportLocation>> GetLocations(Guid importId)
        {
            return await _context.ImportLocations
                .Where(l => l.ImportId == importId)
                .ToListAsync();
        }

        public async Task UpdateCommodity(ImportCommodity commodity)
        {
            _context.ImportCommodities.Update(commodity);
        }

        public async Task UpdateFacility(ImportFacility facility)
        {
            _context.ImportFacilities.Update(facility);
        }

        public async Task UpdateLocation(ImportLocation location)
        {
            _context.ImportLocations.Update(location);
        }
    }
}
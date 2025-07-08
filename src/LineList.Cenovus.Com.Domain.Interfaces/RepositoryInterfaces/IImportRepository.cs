using LineList.Cenovus.Com.API.DataTransferObjects.Import;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportSheet;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportRepository : IRepository<Import>
    {
        new Task<List<Import>> GetAll();
        new Task<Import> GetById(Guid id);
        Task<ImportResultDto> GetWithDetails(Guid id);
        Task<ImportSheetResultDto> GetSheetWithDetails(Guid id);
        Task AddImport(Import import);
        Task UpdateImport(Import import);
        Task AddSheet(ImportSheet sheet);
        Task AddRow(ImportRow row);
        Task UpdateRow(ImportRow row);
        Task AddException(ImportRowException exception);
        Task<bool> LineListExists(string documentNumber);
        Task<bool> LineExists(string lineNo);
        Task AddFacility(ImportFacility facility);
        Task AddCommodity(ImportCommodity commodity);
        Task AddLocation(ImportLocation location);
        Task<List<ImportCommodity>> GetCommodities(Guid importId);
        Task<List<ImportFacility>> GetFacilities(Guid importId);
        Task<List<ImportLocation>> GetLocations(Guid importId);
        Task UpdateCommodity(ImportCommodity commodity);
        Task UpdateFacility(ImportFacility facility);
        Task UpdateLocation(ImportLocation location);
    }
}
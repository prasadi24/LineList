using LineList.Cenovus.Com.API.DataTransferObjects.Import;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportSheet;
using LineList.Cenovus.Com.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportService
    {
        Task<ImportResultDto> GetImportDetails(Guid id);
        Task<ImportSheetResultDto> GetSheetDetails(Guid importSheetId);
        Task<IEnumerable<Import>> GetAll();
        Task<Import> GetById(Guid id);
        Task<Import> Add(Import import);
        Task<Import> Update(Import import);
        Task<bool> Remove(Import import);
        Task<IEnumerable<Import>> Search(string searchCriteria);

        // LDT Import Module-specific methods
        Task<string> ValidateBeforeUpload(IFormFile file);
        Task<Import> Import(IFormFile file, string userName);
        
    }
}
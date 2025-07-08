using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportRowViewService
    {
        Task<IEnumerable<ImportRowView>> GetAll();

        Task<ImportRowView> GetById(Guid id);

        Task<ImportRowView> Add(ImportRowView importRowView);

        Task<ImportRowView> Update(ImportRowView importRowView);

        Task<bool> Remove(ImportRowView importRowView);

        //Task<IEnumerable<ImportRowView>> Search(string searchCriteria);
    }
}
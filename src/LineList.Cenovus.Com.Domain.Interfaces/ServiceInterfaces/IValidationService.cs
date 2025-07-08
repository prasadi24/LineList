using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IValidationService
    {
        Task<IEnumerable<Validation>> GetAll();

        Task<Validation> GetById(Guid id);

        Task<Validation> Add(Validation validation);

        Task<Validation> Update(Validation validation);

        Task<bool> Remove(Validation validation);

        Task<IEnumerable<Validation>> Search(string searchCriteria);
    }
}
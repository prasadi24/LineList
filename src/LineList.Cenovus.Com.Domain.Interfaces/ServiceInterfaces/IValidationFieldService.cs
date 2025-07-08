using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IValidationFieldService
    {
        Task<IEnumerable<ValidationField>> GetAll();

        Task<ValidationField> GetById(Guid id);

        Task<ValidationField> Add(ValidationField validationField);

        Task<ValidationField> Update(ValidationField validationField);

        Task<bool> Remove(ValidationField validationField);

        Task<IEnumerable<ValidationField>> Search(string searchCriteria);
    }
}
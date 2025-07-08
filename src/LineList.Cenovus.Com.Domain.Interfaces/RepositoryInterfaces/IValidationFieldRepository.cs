using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IValidationFieldRepository : IRepository<ValidationField>
    {
        new Task<List<ValidationField>> GetAll();

        new Task<ValidationField> GetById(Guid id);
    }
}
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IValidationRepository : IRepository<Validation>
    {
        new Task<List<Validation>> GetAll();

        new Task<Validation> GetById(Guid id);
    }
}
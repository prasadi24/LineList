using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IUserPreferenceRepository : IRepository<UserPreference>
    {
        new Task<List<UserPreference>> GetAll();

        new Task<UserPreference> GetById(Guid id);
    }
}
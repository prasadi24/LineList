using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IUserPreferenceService
    {
        Task<IEnumerable<UserPreference>> GetAll();

        Task<UserPreference> GetById(Guid id);

        Task<UserPreference> Add(UserPreference userPreference);

        Task<UserPreference> Update(UserPreference userPreference);

        Task<bool> Remove(UserPreference userPreference);

        Task<IEnumerable<UserPreference>> Search(string searchCriteria);
    }
}
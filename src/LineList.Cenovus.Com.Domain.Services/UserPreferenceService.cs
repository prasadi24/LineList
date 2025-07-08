using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly IUserPreferenceRepository _userPreferenceRepository;

        public UserPreferenceService(IUserPreferenceRepository userPreferenceRepository)
        {
            _userPreferenceRepository = userPreferenceRepository;
        }

        public async Task<IEnumerable<UserPreference>> GetAll()
        {
            return await _userPreferenceRepository.GetAll();
        }

        public async Task<UserPreference> GetById(Guid id)
        {
            return await _userPreferenceRepository.GetById(id);
        }

        public async Task<UserPreference> Add(UserPreference userPreference)
        {
            await _userPreferenceRepository.Add(userPreference);
            return userPreference;
        }

        public async Task<UserPreference> Update(UserPreference userPreference)
        {
            await _userPreferenceRepository.Update(userPreference);
            return userPreference;
        }

        public async Task<bool> Remove(UserPreference userPreference)
        {
            await _userPreferenceRepository.Remove(userPreference);
            return true;
        }

        public async Task<IEnumerable<UserPreference>> Search(string searchCriteria)
        {
            return await _userPreferenceRepository.Search(c => c.FullName.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _userPreferenceRepository?.Dispose();
        }
    }
}
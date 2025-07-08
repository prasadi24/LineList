using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;

        public FacilityService(IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
        }

        //public async Task<IEnumerable<Facility>> GetAll()
        //{
        //    return await _facilityRepository.GetAll();
        //}
        public async Task<IEnumerable<Facility>> GetAll()
        {
            var facilities = await _facilityRepository.GetAll();
            return facilities ?? new List<Facility>();
        }

        public async Task<Facility> GetById(Guid id)
        {
            return await _facilityRepository.GetById(id);
        }

        public async Task<Facility> Add(Facility facility)
        {
            if (_facilityRepository.Search(c => c.Name == facility.Name).Result.Any())
                return null;

            await _facilityRepository.Add(facility);
            return facility;
        }

        public async Task<Facility> Update(Facility facility)
        {
            if (_facilityRepository.Search(c => c.Name == facility.Name && c.Id != facility.Id).Result.Any())
                return null;

            await _facilityRepository.Update(facility);
            return facility;
        }

        public async Task<bool> Remove(Facility facility)
        {        
            await _facilityRepository.Remove(facility);
            return true;
        }

        public async Task<IEnumerable<Facility>> Search(string searchCriteria)
        {
            return await _facilityRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _facilityRepository?.Dispose();
        }

        public EpCompanyAlpha GetCompanyAlphaForEPCompany(Guid epCompanyID, Guid facilityId)
        {
            return _facilityRepository.GetCompanyAlphaForEPCompany(epCompanyID, facilityId);
        }

        public bool HasDependencies(Guid facilityId)
        {
            return _facilityRepository.HasDependencies(facilityId);
        }
    }
}
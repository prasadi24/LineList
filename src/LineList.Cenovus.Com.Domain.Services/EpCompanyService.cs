using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpCompanyService : IEpCompanyService
    {
        private readonly IEpCompanyRepository _epCompanyRepository;

        public EpCompanyService(IEpCompanyRepository epCompanyRepository)
        {
            _epCompanyRepository = epCompanyRepository;
        }

        public async Task<IEnumerable<EpCompany>> GetAll()
        {
            return await _epCompanyRepository.GetAll();
        }

        public async Task<EpCompany> GetById(Guid id)
        {
            return await _epCompanyRepository.GetById(id);
        }

        public async Task<EpCompany> Add(EpCompany epCompany)
        {
            if (_epCompanyRepository.Search(c => c.Name == epCompany.Name).Result.Any())
                return null;

            await _epCompanyRepository.Add(epCompany);
            return epCompany;
        }

        public async Task<EpCompany> Update(EpCompany epCompany)
        {
            if (_epCompanyRepository.Search(c => c.Name == epCompany.Name && c.Id != epCompany.Id).Result.Any())
                return null;

            await _epCompanyRepository.Update(epCompany);
            return epCompany;
        }

        public async Task<bool> Remove(EpCompany epCompany)
        {
            await _epCompanyRepository.Remove(epCompany);
            return true;
        }

        public async Task<IEnumerable<EpCompany>> Search(string searchCriteria)
        {
            return await _epCompanyRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _epCompanyRepository?.Dispose();
        }

        public async Task<bool> UpdateCompanyLogo(Guid companyId, string base64String)
        {
            var result = await _epCompanyRepository.UpdateCompanyLogo(companyId, base64String);
            return result;
        }

        public bool HasDependencies(Guid id)
        {
            return _epCompanyRepository.HasDependencies(id);
        }
    }
}
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpCompanyAlphaService : IEpCompanyAlphaService
    {
        private readonly IEpCompanyAlphaRepository _epCompanyAlphaRepository;

        public EpCompanyAlphaService(IEpCompanyAlphaRepository epCompanyAlphaRepository)
        {
            _epCompanyAlphaRepository = epCompanyAlphaRepository;
        }

        public async Task<IEnumerable<EpCompanyAlpha>> GetAll()
        {
            return await _epCompanyAlphaRepository.GetAll();
        }

        public async Task<EpCompanyAlpha> GetById(Guid id)
        {
            return await _epCompanyAlphaRepository.GetById(id);
        }

        public async Task<EpCompanyAlpha> Add(EpCompanyAlpha epCompanyAlpha)
        {
            await _epCompanyAlphaRepository.Add(epCompanyAlpha);
            return epCompanyAlpha;
        }

        public async Task<EpCompanyAlpha> Update(EpCompanyAlpha epCompanyAlpha)
        {
            await _epCompanyAlphaRepository.Update(epCompanyAlpha);
            return epCompanyAlpha;
        }

        public async Task<bool> Remove(EpCompanyAlpha epCompanyAlpha)
        {
            await _epCompanyAlphaRepository.Remove(epCompanyAlpha);
            return true;
        }

        public async Task<IEnumerable<EpCompanyAlpha>> Search(string searchCriteria)
        {
            return await _epCompanyAlphaRepository.Search(c => c.Alpha.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _epCompanyAlphaRepository?.Dispose();
        }
    }
}
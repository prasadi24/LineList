using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class CodeService : ICodeService
    {
        private readonly ICodeRepository _codeRepository;

        public CodeService(ICodeRepository codeRepository)
        {
            _codeRepository = codeRepository;
        }

        public async Task<IEnumerable<Code>> GetAll()
        {
            return await _codeRepository.GetAll();
        }

        public async Task<Code> GetById(Guid id)
        {
            return await _codeRepository.GetById(id);
        }

        public async Task<Code> Add(Code code)
        {
            // Check if a code with the same value exists
            if (_codeRepository.Search(c => c.Name == code.Name).Result.Any())
                return null;

            await _codeRepository.Add(code);
            return code;
        }

        public async Task<Code> Update(Code code)
        {
            // Ensure no duplicate codes with the same value
            if (_codeRepository.Search(c => c.Name == code.Name && c.Id != code.Id).Result.Any())
                return null;

            await _codeRepository.Update(code);
            return code;
        }

        public async Task<bool> Remove(Code code)
        {
            await _codeRepository.Remove(code);
            return true;
        }

        public async Task<IEnumerable<Code>> Search(string searchCriteria)
        {
            return await _codeRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _codeRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _codeRepository.HasDependencies(id);
        }
    }
}
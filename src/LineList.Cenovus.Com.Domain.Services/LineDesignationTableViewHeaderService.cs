using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LineDesignationTableViewHeaderService : ILineDesignationTableViewHeaderService
    {
        private readonly ILineDesignationTableViewHeaderRepository _repository;

        public LineDesignationTableViewHeaderService(ILineDesignationTableViewHeaderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LineDesignationTableViewHeader>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<LineDesignationTableViewHeader> GetById(Guid id)
        {
            return await _repository.GetById(id);
        }

        public async Task<LineDesignationTableViewHeader> Add(LineDesignationTableViewHeader entity)
        {
            if (_repository.Search(c => c.CpName == entity.CpName).Result.Any())
                return null;

            await _repository.Add(entity);
            return entity;
        }

        public async Task<LineDesignationTableViewHeader> Update(LineDesignationTableViewHeader entity)
        {
            if (_repository.Search(c => c.CpName == entity.CpName && c.Id != entity.Id).Result.Any())
                return null;

            await _repository.Update(entity);
            return entity;
        }

        public async Task<bool> Remove(LineDesignationTableViewHeader entity)
        {
            await _repository.Remove(entity);
            return true;
        }

        public async Task<IEnumerable<LineDesignationTableViewHeader>> Search(string searchCriteria)
        {
            return await _repository.Search(c => c.CpName.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }
}
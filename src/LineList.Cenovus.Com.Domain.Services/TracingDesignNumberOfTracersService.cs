using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class TracingDesignNumberOfTracersService : ITracingDesignNumberOfTracersService
    {
        private readonly ITracingDesignNumberOfTracersRepository _tracingDesignNumberOfTracersRepository;

        public TracingDesignNumberOfTracersService(ITracingDesignNumberOfTracersRepository tracingDesignNumberOfTracersRepository)
        {
            _tracingDesignNumberOfTracersRepository = tracingDesignNumberOfTracersRepository;
        }

        public async Task<IEnumerable<TracingDesignNumberOfTracers>> GetAll()
        {
            return await _tracingDesignNumberOfTracersRepository.GetAll();
        }

        public async Task<TracingDesignNumberOfTracers> GetById(Guid id)
        {
            return await _tracingDesignNumberOfTracersRepository.GetById(id);
        }

        public async Task<TracingDesignNumberOfTracers> Add(TracingDesignNumberOfTracers tracingDesignNumberOfTracers)
        {
            // Prevent adding duplicate TracingDesignNumberOfTracers based on design ID and tracer number
            if (_tracingDesignNumberOfTracersRepository.Search(c => c.Name == tracingDesignNumberOfTracers.Name && c.Description == tracingDesignNumberOfTracers.Description).Result.Any())
                return null;  // Prevent duplicate entry

            await _tracingDesignNumberOfTracersRepository.Add(tracingDesignNumberOfTracers);
            return tracingDesignNumberOfTracers;
        }

        public async Task<TracingDesignNumberOfTracers> Update(TracingDesignNumberOfTracers tracingDesignNumberOfTracers)
        {
            // Prevent updating to a duplicate TracingDesignNumberOfTracers for the same design ID
            if (_tracingDesignNumberOfTracersRepository.Search(c => c.Name == tracingDesignNumberOfTracers.Name && c.Description == tracingDesignNumberOfTracers.Description && c.Id != tracingDesignNumberOfTracers.Id).Result.Any())
                return null;  // Prevent duplicate update

            await _tracingDesignNumberOfTracersRepository.Update(tracingDesignNumberOfTracers);
            return tracingDesignNumberOfTracers;
        }

        public async Task<bool> Remove(TracingDesignNumberOfTracers tracingDesignNumberOfTracers)
        {
            await _tracingDesignNumberOfTracersRepository.Remove(tracingDesignNumberOfTracers);
            return true;
        }

        public async Task<IEnumerable<TracingDesignNumberOfTracers>> Search(string searchCriteria)
        {
            return await _tracingDesignNumberOfTracersRepository.Search(c => c.Description.ToString().Contains(searchCriteria));
        }

        public void Dispose()
        {
            _tracingDesignNumberOfTracersRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _tracingDesignNumberOfTracersRepository.HasDependencies(id);
        }
    }
}
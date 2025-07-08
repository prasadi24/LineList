using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IPipeSpecificationService
    {
        Task<IEnumerable<PipeSpecification>> GetAll();

        Task<PipeSpecification> GetById(Guid id);

        Task<PipeSpecification> Add(PipeSpecification pipeSpecification);

        Task<PipeSpecification> Update(PipeSpecification pipeSpecification);

        Task<bool> Remove(PipeSpecification pipeSpecification);

        Task<IEnumerable<PipeSpecification>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}
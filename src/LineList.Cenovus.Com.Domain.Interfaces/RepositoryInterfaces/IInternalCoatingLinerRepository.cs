using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IInternalCoatingLinerRepository : IRepository<InternalCoatingLiner>
{
    new Task<List<InternalCoatingLiner>> GetAll();

    new Task<InternalCoatingLiner> GetById(Guid id);

    bool HasDependencies(Guid id);
}
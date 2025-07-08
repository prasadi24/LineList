using LineList.Cenovus.Com.Domain.Models;


public interface IInternalCoatingLinerService
{
    Task<IEnumerable<InternalCoatingLiner>> GetAll();

    Task<InternalCoatingLiner> GetById(Guid id);

    Task<InternalCoatingLiner> Add(InternalCoatingLiner internalCoatingLiner);

    Task<InternalCoatingLiner> Update(InternalCoatingLiner internalCoatingLiner);

    Task<bool> Remove(InternalCoatingLiner internalCoatingLiner);

    Task<IEnumerable<InternalCoatingLiner>> Search(string searchCriteria);

   bool  HasDependencies(Guid id);
}
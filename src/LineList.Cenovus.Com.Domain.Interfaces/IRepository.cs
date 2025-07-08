using LineList.Cenovus.Com.Domain.Models;
using System.Linq.Expressions;

namespace LineList.Cenovus.Com.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity entity);
        Task AddWithoutSave(TEntity entity);

        Task<List<TEntity>> GetAll();

        Task<TEntity> GetById(Guid id);

        Task Update(TEntity entity);

        Task Remove(TEntity entity);

        Task RemoveWithoutSave(TEntity entity);

        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);

        Task<int> SaveChanges();       
    }
}
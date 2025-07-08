using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly LineListDbContext Db;

        protected readonly DbSet<TEntity> DbSet;

        protected Repository(LineListDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Add(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }
        public virtual async Task AddWithoutSave(TEntity entity)
        {
             DbSet.Add(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remove(TEntity entity)
        {
            DbSet.Remove(entity);
            await SaveChanges();
        }

        public virtual async Task RemoveWithoutSave(TEntity entity)
        {
            var trackedEntity = Db.ChangeTracker.Entries<TEntity>()
                               .FirstOrDefault(e => e.Entity.Equals(entity));

            if (trackedEntity != null)
            {
                trackedEntity.State = EntityState.Detached;
            }
            DbSet.Remove(entity);
        }
        public async Task<int> SaveChanges()
        {
            //try
            //{
                return await Db.SaveChangesAsync();
            //}

            //catch (Exception ex)
            //{

            //}
            //return 0;//await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }      

    }
}
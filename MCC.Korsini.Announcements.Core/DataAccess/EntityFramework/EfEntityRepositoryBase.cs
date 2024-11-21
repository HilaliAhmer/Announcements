using System.Linq.Expressions;
using MCC.Korsini.Announcements.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MCC.Korsini.Announcements.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                // Include işlemleri
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return filter == null
                    ? await query.SingleOrDefaultAsync()  // Filtre yoksa bir kayıt döndür
                    : await query.SingleOrDefaultAsync(filter);  // Filtre varsa ona göre getir
            }
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                // Include işlemleri
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return filter == null
                    ? await query.ToListAsync()  // Filtre yoksa tüm listeyi getir
                    : await query.Where(filter).ToListAsync();  // Filtre varsa ona göre listeyi getir
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            await using var context = new TContext();
            var addedEntity = context.Entry(entity);
            addedEntity.State = EntityState.Added;
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await using var context = new TContext();
            var updatedEntity = context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await using var context = new TContext();
            var deletedEntity = context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

    }
}

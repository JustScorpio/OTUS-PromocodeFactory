using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T>
        : IRepository<T>
        where T: BaseEntity
    {
        protected AppDbContext DbContext;

        public EfRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(DbContext.Set<T>().AsEnumerable<T>());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(DbContext.Set<T>().FirstOrDefault(x => x.Id == id));
        }

        public Task CreateAsync(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return SaveChanges();
        }

        public Task UpdateAsync(T entity)
        {
            var oldEntity = DbContext.Set<T>().FirstOrDefault(x => x.Id == entity.Id);
            oldEntity = entity;
            return SaveChanges();
        }

        public Task RemoveAsync(Guid id)
        {
            var oldEntity = DbContext.Set<T>().Find(id);
            DbContext.Set<T>().Remove(oldEntity);
            return SaveChanges();
        }

        public Task SaveChanges()
        {
            return DbContext.SaveChangesAsync();
        }
    }
}
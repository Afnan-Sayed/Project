using ERP_API.DataAccess.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERP_API.DataAccess.Interfaces;

namespace ERP_API.DataAccess.Repositories
{
    internal class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class
    {
        private readonly ErpDBContext _shoppingDbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(ErpDBContext shoppingDbContext)
        {
            _shoppingDbContext = shoppingDbContext;
            _dbSet = _shoppingDbContext.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public IQueryable<TEntity> GetAllQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public TEntity? FindById(TId id)
        {
            return _dbSet.Find(id);
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public TEntity? Delete(TId id)
        {
            TEntity? entity = FindById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return entity;
            }
            return null;
        }

    }
}

using ERP_DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_DataLayer.Repositories
{
    // Note: We implement your friend's interface exactly
    public class MockBaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class
    {
        // The Memory "Database"
        protected static List<TEntity> _fakeTable = new List<TEntity>();

        public IEnumerable<TEntity> GetAll()
        {
            return _fakeTable;
        }

        public IQueryable<TEntity> GetAllQueryable()
        {
            return _fakeTable.AsQueryable();
        }

        public TEntity? FindById(TId id)
        {
            // REFLECTION MAGIC:
            // Since we don't know if TEntity has an "Id" property at compile time,
            // we look for it dynamically at runtime.
            return _fakeTable.FirstOrDefault(item =>
            {
                var idProperty = item.GetType().GetProperty("Id");
                if (idProperty == null) return false;

                var itemId = idProperty.GetValue(item);
                return itemId != null && itemId.Equals(id);
            });
        }

        public void Create(TEntity entity)
        {
            // Auto-Increment Logic using Reflection
            // We assume TId is an int for this logic. 
            var idProperty = typeof(TEntity).GetProperty("Id");

            if (idProperty != null && idProperty.PropertyType == typeof(int))
            {
                int maxId = 0;
                if (_fakeTable.Any())
                {
                    maxId = _fakeTable.Max(item => (int)idProperty.GetValue(item));
                }
                idProperty.SetValue(entity, maxId + 1);
            }

            _fakeTable.Add(entity);
        }

        public void Update(TEntity entity)
        {
            // In memory lists, if you modify the object reference, it's updated.
            // But usually, we find the old one and replace it.
            var idProperty = typeof(TEntity).GetProperty("Id");
            var id = (TId)idProperty.GetValue(entity);

            var existing = FindById(id);
            if (existing != null)
            {
                _fakeTable.Remove(existing);
                _fakeTable.Add(entity);
            }
        }

        public TEntity? Delete(TId id)
        {
            var entity = FindById(id);
            if (entity != null)
            {
                _fakeTable.Remove(entity);
            }
            return entity;
        }
    }
}

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FHotel.Repository.Models;

namespace FHotel.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly FHotelContext _context;
        private readonly DbSet<T> _table;

        public GenericRepository(FHotelContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public T Find(Func<T, bool> predicate)
        {
            return _table.FirstOrDefault(predicate);
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return
                await _table.SingleOrDefaultAsync(predicate);
        }

        public IQueryable<T> FindAll(Func<T, bool> predicate)
        {
            return _table.Where(predicate).AsQueryable();
        }

        public DbSet<T> GetAll()
        {
            return _table;
        }


        public async Task<T> GetByIdGuid(Guid Id)
        {
            return await _table.FindAsync(Id);
        }

        public async Task<T> GetById(int Id)
        {
            return await _table.FindAsync(Id);
        }

        public async Task HardDeleteGuid(Guid key)
        {
            var rs = await GetByIdGuid(key);
            _table.Remove(rs);
        }

        public async Task HardDelete(int key)
        {
            var rs = await GetById(key);
            _table.Remove(rs);
        }

        public void Insert(T entity)
        {
            _table.Add(entity);
        }

        public async Task UpdateGuid(T entity, Guid Id)
        {
            var existEntity = await GetByIdGuid(Id);
            _context.Entry(existEntity).CurrentValues.SetValues(entity);
            _table.Update(existEntity);
        }

        public async Task Update(T entity, int Id)
        {
            var existEntity = await GetById(Id);
            _context.Entry(existEntity).CurrentValues.SetValues(entity);
            _table.Update(existEntity);
        }


        public void UpdateRange(IQueryable<T> entities)
        {
            _table.UpdateRange(entities);
        }

        public void DeleteRange(IQueryable<T> entities)
        {
            _table.RemoveRange(entities);
        }

        public void InsertRange(IQueryable<T> entities)
        {
            _table.AddRange(entities);
        }

        public EntityEntry<T> Delete(T entity)
        {
            return _table.Remove(entity);
        }

        //async
        public async Task InsertAsync(T entity)
        {
            await _table.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IQueryable<T> entities)
        {
            await _table.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _table.Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAllApart()
        {
            return _table.Take(100);
        }
        public async Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default)
        {
            return await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task UpdateDetached(T entity)
        {
            _table.Update(entity);
        }

        public async Task DetachEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public IQueryable<T> AsNoTracking()
        {
            return _table.AsNoTracking();
        }
    }
}

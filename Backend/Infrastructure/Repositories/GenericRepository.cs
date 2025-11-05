using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly FoodyDbContext _context; 
        private readonly DbSet<TEntity> _dbSet; 

        public GenericRepository(FoodyDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct)
        {
            return await _dbSet.AsNoTracking().ToListAsync(ct);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAsync(int offset, int limit, CancellationToken ct)
        {
            var query = _dbSet.AsNoTracking().Skip(offset).Take(limit).ToListAsync(ct);
            return await query;
        }

        public virtual async Task<TEntity?> GetByIdAsync<TKey>(TKey id, CancellationToken ct)
        {
            return await _dbSet.FindAsync([id], ct);
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken ct)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
           _dbSet.Remove(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }
        public virtual IQueryable<TEntity> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<List<T>?> AddRangeAsync(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public IQueryable<T> AsQueryable()
        {
            return _context.Set<T>().AsNoTracking();
        }



        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T?> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return null;

            var entry = _context.Entry(entity);
            var property = entry.Metadata.FindProperty("IsActive");
            if (property != null && property.ClrType == typeof(bool))
            {
                entry.Property("IsActive").CurrentValue = false;
                entry.State = EntityState.Modified;
            }
            return entity;
        }

        public async Task<T?> UpdateAsync(Guid id, T entity)
        {
            var existing = await _context.Set<T>().FindAsync(id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            return existing;
        }
    }
}
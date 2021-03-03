using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OTC.Employees.Test.Models;

namespace OTC.Employees.Test.Data
{
	public class BaseRepository<T> : IDisposable, IRepository<T> where T : DbEntry
	{
		private readonly DbSet<T> _table;

		public BaseRepository() : this(new AppDbContext())
		{
		}

		public BaseRepository(AppDbContext context)
		{
			Context = context;
			_table = Context.Set<T>();
		}

		protected AppDbContext Context { get; }

		public void Dispose()
		{
			Context?.Dispose();
		}

		public async Task<bool> TryDelete(int? id)
		{
			if (id is null || id < 1) return false;

			var e = await _table.FindAsync(id);
			if (e is null) return false;

			Context.Entry(e).State = EntityState.Deleted;
			await Context.SaveChangesAsync();
			return true;
		}

		public virtual async Task Update(T entity)
		{
			Context.Entry(entity).State = EntityState.Modified;
			await Save(entity);
		}

		public virtual async Task<T> GetOne(int? id)
		{
			return await _table.FindAsync(id);
		}

		public virtual async Task<List<T>> GetAll()
		{
			return await _table.ToListAsync();
		}

		public virtual async Task<bool> IsExists(T entity)
		{
			return await _table.FindAsync(entity.Id) != null;
		}

		public async Task Save(T entity)
		{
			await Context.SaveChangesAsync();
		}

		public async Task Add(T entity)
		{
			await _table.AddAsync(entity);
			await Save(entity);
		}
	}
}
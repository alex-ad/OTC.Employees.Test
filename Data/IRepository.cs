using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTC.Employees.Test.Data
{
	public interface IRepository<T>
	{
		Task Update(T entity);
		Task Add(T entity);
		Task<bool> TryDelete(int? id);
		Task<T> GetOne(int? id, string includeProperties = "");
		Task<List<T>> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
		Task<bool> IsExists(T entity);
		Task Save(T entity);
	}
}
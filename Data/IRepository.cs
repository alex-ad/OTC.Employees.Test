using System.Collections.Generic;
using System.Threading.Tasks;

namespace OTC.Employees.Test.Data
{
	public interface IRepository<T>
	{
		Task Update(T entity);
		Task Add(T entity);
		Task<bool> TryDelete(int? id);
		Task<T> GetOne(int? id);
		Task<List<T>> GetAll();
		Task<bool> IsExists(T entity);
		Task Save(T entity);
	}
}
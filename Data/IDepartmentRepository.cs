using System.Threading.Tasks;
using OTC.Employees.Test.Models;

namespace OTC.Employees.Test.Data
{
	public interface IDepartmentRepository : IRepository<Department>
	{
		Task<Department> GetOne(string name);
	}
}
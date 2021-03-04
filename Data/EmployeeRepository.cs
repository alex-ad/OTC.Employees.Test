using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OTC.Employees.Test.Models;

namespace OTC.Employees.Test.Data
{
	public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(AppDbContext context) : base(context)
		{
		}

		public override async Task Update(Employee entity)
		{
			if (entity.Id == 0)
			{
				await Add(entity);
				return;
			}

			var e = await base.GetOne(entity.Id);
			if (e is null)
				return;
			e.Name = entity.Name;
			e.Salary = entity.Salary;
			e.DepartmentId = entity.DepartmentId;

			await base.Update(e);
		}
	}
}
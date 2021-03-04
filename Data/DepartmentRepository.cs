using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OTC.Employees.Test.Models;

namespace OTC.Employees.Test.Data
{
	public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
	{
		public DepartmentRepository(AppDbContext context) : base(context)
		{
		}

		public override async Task Update(Department entity)
		{
			if (entity.Id == 0)
			{
				await Add(entity);
				return;
			}

			var e = await base.GetOne(entity.Id);
			if (e is null) return;
			e.Name = entity.Name;

			await base.Update(e);
		}

		public override async Task<bool> IsExists(Department department)
		{
			return await Context.Departments.FirstOrDefaultAsync(x =>
				x.Id != department.Id && string.Equals(x.Name, department.Name)) != null;
		}

		public async Task<Department> GetOne(string name)
		{
			return await Context.Departments.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Name == name);
		}
	}
}
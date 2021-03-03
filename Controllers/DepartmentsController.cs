using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OTC.Employees.Test.Data;
using OTC.Employees.Test.Models;

namespace OTC.Employees.Test.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentsController : ControllerBase
	{
		private readonly IDepartmentRepository _departmentRepo;

		public DepartmentsController(IDepartmentRepository departmentRepo)
		{
			_departmentRepo = departmentRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
		{
			return await _departmentRepo.GetAll();
		}

		[HttpGet("id/{id}")]
		public async Task<ActionResult<Department>> GetDepartmentById(int id)
		{
			var dept = await _departmentRepo.GetOne(id);
			return dept is null ? BadRequest() : dept;
		}

		[HttpGet("name/{name}")]
		public async Task<ActionResult<Department>> GetDepartmentByName(string name)
		{
			var dept = await _departmentRepo.GetOne(name);
			return dept is null ? BadRequest() : dept;
		}
	}
}
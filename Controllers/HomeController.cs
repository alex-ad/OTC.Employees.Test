using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using OTC.Employees.Test.Data;
using OTC.Employees.Test.Models;

namespace OTC.Employees.Test.Controllers
{
	public class HomeController : Controller
	{
		private readonly IDepartmentRepository _departmentRepo;
		private readonly IEmployeeRepository _employeeRepo;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, IEmployeeRepository employeeRepo,
			IDepartmentRepository departmentRepo)
		{
			_logger = logger;
			_departmentRepo = departmentRepo;
			_employeeRepo = employeeRepo;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}

		#region Departments

		[HttpGet]
		public async Task<IActionResult> DepartmentsList()
		{
			return View(await _departmentRepo.GetAll(orderBy: q => q.OrderBy(d => d.Name), includeProperties: "Employees"));
		}

		[HttpGet]
		public async Task<IActionResult> DepartmentEdit(int? id)
		{
			if (id == 0) return View(new Department());

			var dept = await _departmentRepo.GetOne(id, "Employees");
			return dept is null ? BadRequest() : View(dept);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DepartmentEdit(Department department)
		{
			if (!ModelState.IsValid) return View(department);

			if (await _departmentRepo.IsExists(department))
			{
				ModelState.AddModelError(nameof(department.Name), "Заданное имя уже существует");
				return View(department);
			}

			await _departmentRepo.Update(department);
			return RedirectToAction("DepartmentsList");
		}

		[HttpGet]
		public IActionResult DepartmentCreate()
		{
			return RedirectToAction("DepartmentEdit", new {id = 0});
		}

		[HttpGet]
		public async Task<IActionResult> DepartmentDetails(int? id)
		{
			var dept = await _departmentRepo.GetOne(id, "Employees");
			return dept is null ? BadRequest() : View(dept);
		}

		[HttpGet]
		public async Task<IActionResult> DepartmentDelete(int? id)
		{
			return await _departmentRepo.TryDelete(id) ? RedirectToAction("DepartmentsList") : BadRequest();
		}

		#endregion

		#region Employees

		[HttpGet]
		public async Task<IActionResult> EmployeesList()
		{
			return View(await _employeeRepo.GetAll(orderBy: q => q.OrderBy(e => e.Name), includeProperties: "Department"));
		}

		[HttpGet]
		public async Task<IActionResult> EmployeeDelete(int? id)
		{
			return await _employeeRepo.TryDelete(id) ? RedirectToAction("EmployeesList") : BadRequest();
		}

		[HttpGet]
		public async Task<IActionResult> EmployeeDetails(int? id)
		{
			var empl = await _employeeRepo.GetOne(id, "Department");
			return empl is null ? BadRequest() : View(empl);
		}

		[HttpGet]
		public async Task<IActionResult> EmployeeEdit(int? id)
		{
			var depts = await _departmentRepo.GetAll();
			if (depts.Count < 1) return RedirectToAction("DepartmentEdit", new {id = 0});

			ViewBag.Departments = new SelectList(depts, "Id", "Name");

			if (id == 0)
				return View(new Employee());

			var empl = await _employeeRepo.GetOne(id, "Department");
			return empl is null ? BadRequest() : View(empl);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EmployeeEdit(Employee employee)
		{
			if (!ModelState.IsValid)
				return View(employee);

			await _employeeRepo.Update(employee);
			return RedirectToAction("EmployeesList");
		}

		[HttpGet]
		public IActionResult EmployeeCreate()
		{
			return RedirectToAction("EmployeeEdit", new {id = 0});
		}

		#endregion
	}
}
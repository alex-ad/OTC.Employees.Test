using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OTC.Employees.Test.Models;

namespace OTC.Employees.Test.Data
{
	public static class SeedData
	{
		public static void EnsurePopulated(IApplicationBuilder app, string connString)
		{
			var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

			context.Database.EnsureDeleted();

			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();

				var d1 = new Department {Name = "Отдел 1", Salary = 23833};
				var d2 = new Department {Name = "Отдел 2", Salary = 55100};
				var d3 = new Department {Name = "Отдел 3", Salary = 114000};
				var d4 = new Department {Name = "Отдел 4", Salary = 43250};
				var d5 = new Department {Name = "Отдел 5", Salary = 74600};

				var e1 = new Employee {Name = "Aaa Aaa Aaa", Salary = 20000, Department = d1};
				var e2 = new Employee {Name = "Ббб Ббб Ббб", Salary = 21000, Department = d1};
				var e3 = new Employee {Name = "Ввв Ввв Ввв", Salary = 21000, Department = d1};
				var e4 = new Employee {Name = "Ггг Ггг Ггг", Salary = 24000, Department = d1};
				var e5 = new Employee {Name = "Ддд Ддд Ддд", Salary = 27000, Department = d1};
				var e6 = new Employee {Name = "Еее Еее Еее", Salary = 30000, Department = d1};
				var e7 = new Employee {Name = "Ёёё Ёёё Ёёё", Salary = 50000, Department = d2};
				var e8 = new Employee {Name = "Жжж Жжж Жжж", Salary = 50000, Department = d2};
				var e9 = new Employee {Name = "Ззз Ззз Ззз", Salary = 53000, Department = d2};
				var e10 = new Employee {Name = "Иии Иии Иии", Salary = 53000, Department = d2};
				var e11 = new Employee {Name = "Ййй Ййй Ййй", Salary = 54000, Department = d2};
				var e12 = new Employee {Name = "Ккк Ккк Ккк", Salary = 54000, Department = d2};
				var e13 = new Employee {Name = "Ллл Ллл Ллл", Salary = 54000, Department = d2};
				var e14 = new Employee {Name = "Ммм Ммм Ммм", Salary = 60000, Department = d2};
				var e15 = new Employee {Name = "Ннн Ннн Ннн", Salary = 60000, Department = d2};
				var e16 = new Employee {Name = "Ооо Ооо Ооо", Salary = 63000, Department = d2};
				var e17 = new Employee {Name = "Ппп Ппп Ппп", Salary = 100000, Department = d3};
				var e18 = new Employee {Name = "Ррр Ррр Ррр", Salary = 100000, Department = d3};
				var e19 = new Employee {Name = "Ссс Ссс Ссс", Salary = 110000, Department = d3};
				var e20 = new Employee {Name = "Ттт Ттт Ттт", Salary = 110000, Department = d3};
				var e21 = new Employee {Name = "Ууу Ууу Ууу", Salary = 150000, Department = d3};
				var e22 = new Employee {Name = "Ффф Ффф Ффф", Salary = 40000, Department = d4};
				var e23 = new Employee {Name = "Ххх Ххх Ццц", Salary = 40000, Department = d4};
				var e24 = new Employee {Name = "Ччч Ччч Ччч", Salary = 45000, Department = d4};
				var e25 = new Employee {Name = "Шшш Шшш Шшш", Salary = 48000, Department = d4};
				var e26 = new Employee {Name = "Щщщ Щщщ Щщщ", Salary = 70000, Department = d5};
				var e27 = new Employee {Name = "ЭЭэ Эээ Эээ", Salary = 70000, Department = d5};
				var e28 = new Employee {Name = "Ююю Ююю Ююю", Salary = 75000, Department = d5};
				var e29 = new Employee {Name = "Яяя Яяя Яяя", Salary = 78000, Department = d5};
				var e30 = new Employee {Name = "Ььь Ььь Ььь", Salary = 80000, Department = d5};

				context.Departments.AddRange(d1, d2, d3, d4, d5);

				context.Employees.AddRange(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10,
					e11, e12, e13, e14, e15, e16, e17, e18, e19, e20,
					e21, e22, e23, e24, e25, e26, e27, e28, e29, e30);

				context.SaveChanges();
				CreateTrigger(connString);
			}
			else
			{
				throw new Exception("No migrations was found");
			}
		}

		private static void CreateTrigger(string connString)
		{
			var cmd_upd =
				"CREATE TRIGGER Salary_Update ON Employees AFTER UPDATE AS UPDATE Departments SET Salary = (SELECT AVG(e.Salary) FROM Employees AS e WHERE e.DepartmentId = Departments.Id) WHERE Id = (SELECT DepartmentId FROM inserted) OR Id = (SELECT DepartmentId FROM deleted)";
			var cmd_ins =
				"CREATE TRIGGER Salary_Insert ON Employees AFTER INSERT AS UPDATE Departments SET Salary = (SELECT AVG(e.Salary) FROM Employees AS e WHERE e.DepartmentId = (SELECT DepartmentId FROM inserted)) WHERE Id = (SELECT DepartmentId FROM inserted)";
			var cmd_del =
				"CREATE TRIGGER Salary_Delete ON Employees AFTER DELETE AS UPDATE Departments SET Salary = (SELECT AVG(e.Salary) FROM Employees AS e WHERE e.DepartmentId = (SELECT DepartmentId FROM deleted)) WHERE Id = (SELECT TOP 1 DepartmentId FROM deleted)";

			using var connection = new SqlConnection(connString);
			connection.Open();
			var commandUpd = new SqlCommand(cmd_upd, connection);
			var commandDel = new SqlCommand(cmd_del, connection);
			var commanIns = new SqlCommand(cmd_ins, connection);
			commandUpd.ExecuteNonQuery();
			commandDel.ExecuteNonQuery();
			commanIns.ExecuteNonQuery();
		}
	}
}
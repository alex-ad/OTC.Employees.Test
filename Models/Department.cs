using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OTC.Employees.Test.Models
{
	public class Department : DbEntry
	{
		public Department()
		{
			Employees = new List<Employee>();
		}

		[Required]
		[MinLength(2, ErrorMessage = "Имя должно содержать от двух символов")]
		[DisplayName("Название отдела")]
		public string Name { get; set; }

		[DefaultValue(0)]
		[Column(TypeName = "decimal(8, 2)")]
		[DisplayName("Средняя зарплата")]
		public double? Salary { get; set; }

		[DisplayName("Список сотрудников")] public List<Employee> Employees { get; set; }

		[NotMapped]
		[DisplayName("Количество сотрудников")]
		public int EmployeeCount => Employees.Count();
	}
}
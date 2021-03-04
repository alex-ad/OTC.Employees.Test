using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OTC.Employees.Test.Models
{
	public class Employee : DbEntry
	{
		[Required]
		[MinLength(2, ErrorMessage = "Имя должно содержать от двух символов")]
		[DisplayName("ФИО")]
		public string Name { get; set; }

		[Range(1, 999_999_999, ErrorMessage = "Введите положительное число больше от 0 до 999 999 999")]
		[DefaultValue(1)]
		[Required]
		[Column(TypeName = "decimal(11, 2)")]
		[DisplayName("Зарплата")]
		public double Salary { get; set; }

		[Required] public int DepartmentId { get; set; }

		[DisplayName("Подразделение")] public Department Department { get; set; }
	}
}
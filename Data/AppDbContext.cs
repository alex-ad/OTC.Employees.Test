using Microsoft.EntityFrameworkCore;
using OTC.Employees.Test.Models;

namespace OTC.Employees.Test.Data
{
	public class AppDbContext : DbContext
	{
		internal AppDbContext()
		{
		}

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Department> Departments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Department>()
				.HasIndex(p => new {p.Name})
				.IsUnique();
		}
	}
}
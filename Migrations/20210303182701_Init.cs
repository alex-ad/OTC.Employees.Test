using Microsoft.EntityFrameworkCore.Migrations;

namespace OTC.Employees.Test.Migrations
{
	public partial class Init : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"Departments",
				table => new
				{
					Id = table.Column<int>("int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>("nvarchar(450)", nullable: false),
					Salary = table.Column<decimal>("decimal(8,2)", nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_Departments", x => x.Id); });

			migrationBuilder.CreateTable(
				"Employees",
				table => new
				{
					Id = table.Column<int>("int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>("nvarchar(max)", nullable: false),
					Salary = table.Column<decimal>("decimal(8,2)", nullable: false),
					DepartmentId = table.Column<int>("int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Employees", x => x.Id);
					table.ForeignKey(
						"FK_Employees_Departments_DepartmentId",
						x => x.DepartmentId,
						"Departments",
						"Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				"IX_Departments_Name",
				"Departments",
				"Name",
				unique: true);

			migrationBuilder.CreateIndex(
				"IX_Employees_DepartmentId",
				"Employees",
				"DepartmentId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"Employees");

			migrationBuilder.DropTable(
				"Departments");
		}
	}
}
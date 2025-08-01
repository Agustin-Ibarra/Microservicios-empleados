using System.Text;
using app.controller;
using app.Model;
using app.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests;

public class EmployeeControllerTest
{
	[Fact]
	public async Task EmployeeController_RetunrOk()
	{
		// Averrage
		var employee = new EmployeeModel { email = "dpeartree0@samsung.com", first_name = "David", last_name = "Johnson" };
		var employeeRepositoryMock = new Mock<IEmployeeRepository>();
		string rows = "first_name,last_name,email,phone,job_title\nDavid,Johnson,dpeartree0@samsung.com,401-185-8391,Frontend Developer";
		var stream = new MemoryStream(Encoding.UTF8.GetBytes(rows));
		IFormFile employeesCsv = new FormFile(stream, 0, stream.Length, "employees", "employees.csv");
		employeeRepositoryMock
		.Setup(employee => employee.CreateEmployee(new EmployeeModel
		{
			email = "employeeTestgmail.com",
			first_name = "First name",
			last_name = "Last name"
		}));

		var controller = new EmployeeController(employeeRepositoryMock.Object);
		// Request
		var request = await controller.GetEmployees(employeesCsv);
		// Assert
		var response = Assert.IsType<OkObjectResult>(request);
		var objectResponse = Assert.IsType<ErrorModel>(response.Value);
		Assert.Equal(200, response.StatusCode);
		Assert.Equal(0, objectResponse.TotalErrors);
	}
}
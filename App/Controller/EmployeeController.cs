using app.Model;
using app.Repository;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace app.controller;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixedWindows")]
public class EmployeeController : ControllerBase
{
  private readonly IEmployeeRepository _employeeRepository;
  public EmployeeController(IEmployeeRepository employeeRepository)
  {
    _employeeRepository = employeeRepository;
  }

  [Authorize]
  [HttpPost]
  public async Task<ActionResult> GetEmployees(IFormFile employeeCsv)
  {
    var stream = employeeCsv.OpenReadStream();
    var reader = new StreamReader(stream);
    var content = reader.ReadToEnd();
    string[] rows = content.Split("\n"); // las filas se obtienen separando el archivo por cada salto de linea
    List<DetailErrorModel> errors = [];
    for (int i = 0; i < rows.Length; i++)
    {
      if (i > 0 && rows[i] != "") // en el indece 0 los valore representan las cabeceras
      {
        int index = i + 1; // representa el numero de fila
        int errorCount = errors.Count;
        string[] column = rows[i].Split(","); // las columnas se obtienen separando las filas mediante las comas
        string firstName = column[0];
        string lastName = column[1];
        string email = column[2];
        string phoneString = column[3];
        string job = column[4];
        long phoneNumber = 0;
        int idJob = 0;
        try
        {
          FileService.ValidateData(column);
          FileService.ValidateEmail(email);
          idJob = FileService.GetJobId(job);
          phoneNumber = FileService.ValidatePhoneNumber(phoneString);
        }
        catch (Exception ex)
        {
          var error = new DetailErrorModel
          {
            Row = index,
            MessageError = ex.Message
          };
          errors.Add(error);
        }
        if (errorCount == errors.Count)
        {
          var employee = new EmployeeModel
          {
            First_name = firstName,
            Last_name = lastName,
            Email = email,
            id_job = idJob,
            Phone = phoneNumber
          };
          try
          {
            await _employeeRepository.CreateEmployee(employee);
          }
          catch (Exception ex)
          {
            var error = new DetailErrorModel
            {
              MessageError = ex.Message,
              Row = index
            };
            errors.Add(error);
          }
        }
      }
    }
    return Ok(new ErrorModel { TotalRows = rows.Length, TotalErrors = errors.Count, ErrorsList = errors });
  }
}
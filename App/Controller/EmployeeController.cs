using app.Model;
using app.Repository;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.controller;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
  private readonly IEmployeeRepository _employeeRepository;
  public EmployeeController(IEmployeeRepository employeeRepository)
  {
    _employeeRepository = employeeRepository;
  }

  [HttpPost]
  public async Task<ActionResult> GetEmployees(IFormFile employeeCsv)
  {
    var stream = employeeCsv.OpenReadStream();
    var reader = new StreamReader(stream);
    var content = reader.ReadToEnd();
    string[] rows = content.Split("\n"); // las filas se obtienen separando el archivo por cada salto de linea
    List<ErrorModel> errors = [];
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
          var error = new ErrorModel
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
            var error = new ErrorModel
            {
              MessageError = ex.Message,
              Row = index
            };
            errors.Add(error);
          }
        }
      }
    }
    return Ok(new { totalRows = rows.Length, totalErrors = errors.Count, errorsList = errors });
  }
}
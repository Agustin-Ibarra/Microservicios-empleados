using System.ComponentModel.DataAnnotations;

namespace MicroserviceEmployee.Api.Model;

public class EmployeeModel
{
  [Key]
  public int id_employee { get; set; }
  public required string first_name { get; set; }
  public required string last_name { get; set; }
  public required string email { get; set; }
  public long phone { get; set; }
  public int id_job { get; set; }
}
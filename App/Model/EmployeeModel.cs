using System.ComponentModel.DataAnnotations;

namespace app.Model;

public class EmployeeModel
{
  [Key]
  public int id_employee { get; set; }
  public required string First_name { get; set; }
  public required string Last_name { get; set; }
  public required string Email { get; set; }
  public long Phone { get; set; }
  public int id_job { get; set; }
}
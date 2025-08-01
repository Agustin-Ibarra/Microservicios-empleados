using app.Data;
using app.Model;
using Microsoft.EntityFrameworkCore;

namespace app.Repository;

public interface IEmployeeRepository
{
  Task CreateEmployee(EmployeeModel employee);
}

public class EmployeeRepository : IEmployeeRepository
{
  private readonly AppDbContext _context;
  public EmployeeRepository(AppDbContext context)
  {
    _context = context;
  }
  public async Task CreateEmployee(EmployeeModel employee)
  {
    try
    {
      _context.Employees.Add(employee);
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
      throw new Exception(message: $"El email {employee.email} ya esta registrado");
    }
  }
}
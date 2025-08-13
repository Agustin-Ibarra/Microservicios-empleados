using MicroserviceEmployee.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceEmployee.Api.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
  public DbSet<EmployeeModel> Employees { get; set; }
}
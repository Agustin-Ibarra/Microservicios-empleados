using app.Model;
using Microsoft.EntityFrameworkCore;

namespace app.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
  public DbSet<EmployeeModel> Employees { get; set; }
}
using app.Data;
using app.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connection = Environment.GetEnvironmentVariable("DB_CONNECTION");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
  options.AddPolicy("Fetch", policy =>
  {
    policy.WithOrigins("null")
    .AllowAnyHeader()
    .AllowAnyMethod();
  });
});

if (connection != null)
{
  builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connection));
  builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
}
else
{
  Console.WriteLine("connection fail");
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();
app.Run();
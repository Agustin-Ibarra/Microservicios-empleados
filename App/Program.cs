using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using app.Data;
using app.Logs;
using app.Repository;
using Microsoft.EntityFrameworkCore;
using Google.Protobuf.WellKnownTypes;

var builder = WebApplication.CreateBuilder(args);
var connection = Environment.GetEnvironmentVariable("DB_CONNECTION");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddRateLimiter(options =>
{
  options.AddPolicy("fixedWindows", context => RateLimitPartition.GetFixedWindowLimiter(
    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown ip",
    factory: key => new FixedWindowRateLimiterOptions
    {
      PermitLimit = 60,
      Window = TimeSpan.FromSeconds(60),
      QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
      QueueLimit = 0
    }
  ));
});
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
  Console.WriteLine("DB connection fail or connection string is null");
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
app.UseMiddleware<Loggin>();
app.UseRateLimiter();
app.MapControllers();
app.Run();
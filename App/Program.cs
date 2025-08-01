using System.Text;
using System.Threading.RateLimiting;
using app.Data;
using app.Logs;
using app.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var connection = Environment.GetEnvironmentVariable("DB_CONNECTION");
var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddRateLimiter(options =>
{
  options.AddPolicy("fixedWindows", context => RateLimitPartition.GetFixedWindowLimiter(
    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown ip",
    factory: key => new FixedWindowRateLimiterOptions
    {
      PermitLimit = 15,
      Window = TimeSpan.FromSeconds(60),
      QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
      QueueLimit = 0
    }
  ));
  options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
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
if (secret != null)
{
  builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
      ValidateIssuer = false,
      ValidateAudience = false
    };
  });
}
else{
  throw new Exception(message : "jwt secret key is null");
}
if (connection != null)
{
  builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
  builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
}
else
{
  throw new Exception(message : "DB connection fail or connection string is null");
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
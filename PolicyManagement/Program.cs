
using PolicyManagement.Services;
using PolicyManagement.Repositories;
using PolicyManagementApp.Data; 
using Microsoft.EntityFrameworkCore;
using policyManagementApp.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;  
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PolicyManagement.Middleware;
using Capstone_dotnet.Logging;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PolicyManagementDbConnectionString")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<GlobalResponseFilter>();
builder.Services.AddScoped<ResponseTimeFilter>();
builder.Services.AddScoped<IPolicyEnrollmentRepo, PolicyEnrollmentRepo>();
builder.Services.AddScoped<IPolicyEnrollment, PolicyEnrollment>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddHttpContextAccessor();
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
var key = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddScoped<ILogin, Login>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),

        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();


var app = builder.Build();

// Register CorrelationIdMiddleware first (before exception handling)
app.UseMiddleware<CorrelationIdMiddleware>();

// Register exception handling middleware as the first middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
using FungEyeApi.Data;
using FungEyeApi.Interfaces;
using FungEyeApi.Middleware;
using FungEyeApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();


// Configure Kestrel based on environment
if (builder.Environment.IsProduction())
{
    // Production (Azure): HTTPS
    builder.WebHost.ConfigureKestrel(options =>
  {
      options.ListenAnyIP(80);
      options.ListenAnyIP(443, listenOptions =>
      {
          listenOptions.UseHttps("/etc/ssl/certs/certificate.pfx", builder.Configuration.GetSection("SSL_password").Value!);
      });
  });
}
else
{
    // Development: HTTP only
    builder.WebHost.ConfigureKestrel(options =>
  {
      options.ListenAnyIP(80);
  });
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    });

    //options.UseSqlite(@"Data Source=../mydatabase.db");
});


builder.Services.AddHostedService<FungEyeBackgroundService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<IFungiAtlasService, FungiAtlasService>();
builder.Services.AddScoped<IPostsService, PostsService>();

var app = builder.Build();

app.UseMetricServer();
app.UseMiddleware<MetricsMiddleware>();
app.UseHttpMetrics();

app.UseCors();

app.UseSwagger();
//app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
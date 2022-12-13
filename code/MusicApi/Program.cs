using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicApi.Data;
using MusicApi.Filters;
using MusicApi.Helpers;
using MusicApi.Services.UserService;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureMiddleware(app, app.Environment);
ConfigureDatabases(app.Services);
ConfigureEndpoints(app);

app.Run();


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
  services.AddScoped<IUserService, UserService>();
  services.AddSingleton<IFormFileUploader, FormFileUploader>();
  services.AddHttpContextAccessor(); // Adds default implementation to the HttpContextAccessor

  // NOTE: Validation using filter attribute.
  services.AddScoped<ValidationFilterAttribute>();

  services.AddControllers();
  // NOTE: Validation setting options
  // services.AddControllers(
  //   options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = false);

  services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters()
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value ?? "")),
      ValidateIssuer = false,
      ValidateAudience = false,
    };
  });

  services.AddMvc().AddXmlSerializerFormatters();
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  services.AddEndpointsApiExplorer();
  services.AddSwaggerGen(options =>
  {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
      Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
      In = ParameterLocation.Header,
      Name = "Authorization",
      Type = SecuritySchemeType.ApiKey,
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
  });
  services.AddDbContext<MusicApiDbContext>(options =>
  {
    options.UseNpgsql(configuration.GetConnectionString("MusicDbConnection")
      ?? throw new InvalidOperationException("Missing MusicDbConnection connection string"));
  });
}

void ConfigureMiddleware(IApplicationBuilder appBuilder, IWebHostEnvironment env)
{
  // Configure the HTTP request pipeline.
  if (env.IsDevelopment())
  {
    appBuilder.UseDeveloperExceptionPage();
    appBuilder.UseSwagger();
    appBuilder.UseSwaggerUI();
  }

  appBuilder.UseHttpsRedirection();
  appBuilder.UseAuthentication(); // Make sure is before UseAuthorization()
  appBuilder.UseAuthorization();
}

void ConfigureDatabases(IServiceProvider serviceProvider)
{
  // var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
  // using (var serviceScope = serviceScopeFactory.CreateScope())
  // {
  //   var dbContext = serviceScope?.ServiceProvider.GetService<MusicApiDbContext>();
  //   dbContext?.Database.EnsureCreated();
  // }
}

void ConfigureEndpoints(IEndpointRouteBuilder app)
{
  app.MapControllers();
}
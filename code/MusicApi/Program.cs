using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureMiddleware(app, app.Environment);
ConfigureDatabases(app.Services);
ConfigureEndpoints(app);

app.Run();


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
  services.AddSingleton<IFormFileUploader, FormFileUploader>();

  services.AddControllers();
  services.AddMvc().AddXmlSerializerFormatters();
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  services.AddEndpointsApiExplorer();
  services.AddSwaggerGen();

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
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using EtteplanMORE.ServiceManual.ApplicationCore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFactoryDeviceService, FactoryDeviceService>();
builder.Services.AddScoped<IMaintenanceTaskService, MaintenanceTaskService>();

string connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("EtteplanMORE.ServiceManual.Web")));

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();

    var deviceService = scope.ServiceProvider.GetRequiredService<IFactoryDeviceService>();
    await deviceService.SeedData(dbContext); // Seed device data
    await deviceService.SeedTaskData(dbContext); // Seed example task data
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

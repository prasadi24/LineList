using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Services;
using LineList.Cenovus.Com.UI;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddSingleton<SessionTracker>();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

var env = app.Environment;
startup.Configure(app, env);

// ✅ Parameterless method
app.UseRotativa();

// ✅ Set static config for binary path
RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

app.Run();

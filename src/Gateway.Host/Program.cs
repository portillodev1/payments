using BuildingBlocks.Abstractions;
using Payments.Module;
using Processing.Module;

var builder = WebApplication.CreateBuilder(args);

IModule[] all = [new PaymentsModule(), new ProcessingModule()];
var enabled = builder.Configuration.GetSection("Modules:Enabled").Get<string[]>() ?? [];
var modules = all.Where(m => enabled.Contains(m.Name, StringComparer.OrdinalIgnoreCase)).ToArray();

// builder.Services.AddKafkaMessaging(builder.Configuration);   // BuildingBlocks.Messaging, entra na F1-04
foreach (var m in modules) m.Register(builder.Services, builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapOpenApi();
app.MapHealthChecks("/health");
foreach (var m in modules) m.MapEndpoints(app);
app.Run();

public partial class Program;
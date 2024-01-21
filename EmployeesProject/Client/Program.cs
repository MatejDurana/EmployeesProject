using EmployeesProject.Client;
using EmployeesProject.Client.Services.EmployeeServices;
using EmployeesProject.Client.Services.PositionServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IEmployeeService, ClientEmployeeService>();
builder.Services.AddScoped<IPositionService, ClientPositionService>();


await builder.Build().RunAsync();

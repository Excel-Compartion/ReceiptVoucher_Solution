using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using ReceiptVoucher.Client.Pages;
//using ReceiptVoucher.Components;
using ReceiptVoucher.Core;
using ReceiptVoucher.Core.Interfaces;
using ReceiptVoucher.EF;
using ReceiptVoucher.EF.Repositories;
using ReceiptVoucher.Server.Components;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddScoped<HttpClient>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddTransient<ISubProjectRepository, SubProjectRepository>();
builder.Services.AddTransient<IReceiptRepository, ReceiptRepository>();


builder.Services.AddMudServices();  // MudBlazor
builder.Services.AddAutoMapper(typeof(Program));    // add AutoMapper.


var connectionString = builder.Configuration.GetConnectionString("DefualtConnection")
            ?? throw new InvalidOperationException("No Connection String Was Found");

builder.Services.AddDbContext<ReceiptVoucherDbContext>(options =>
        options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(ReceiptVoucherDbContext).Assembly.FullName))); // Register DbContext 

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();    // Register IUnitOfWork


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

// Configure the HTTP request pipeline.
app.MapControllers();



app.Run();

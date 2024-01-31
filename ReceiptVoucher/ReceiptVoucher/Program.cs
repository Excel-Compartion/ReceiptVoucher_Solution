

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using ReceiptVoucher.Client.Pages;
using ReceiptVoucher.Client.Services;

//using ReceiptVoucher.Components;
using ReceiptVoucher.Core;
using ReceiptVoucher.Core.Helper;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Interfaces;
using ReceiptVoucher.Core.Services;
using ReceiptVoucher.EF;
using ReceiptVoucher.EF.Repositories;
using ReceiptVoucher.Server.Components;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddScoped<HttpClient>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
builder.Services.AddBlazoredLocalStorage(); // added local storage for jwt


builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


// here inject all Services --------

builder.Services.AddTransient<ISubProjectRepository, SubProjectRepository>();
builder.Services.AddTransient<IReceiptRepository, ReceiptRepository>();

builder.Services.AddTransient<IProjectRepository, ProjectRepository>();

builder.Services.AddMudServices();  // MudBlazor
builder.Services.AddAutoMapper(typeof(Program));    // add AutoMapper.

builder.Services.AddScoped<ReceiptVoucher.Core.Services.IAuthService, ReceiptVoucher.Core.Services.AuthService>(); // server

builder.Services.AddScoped<ReceiptVoucher.Client.Services.IAuthService, ReceiptVoucher.Client.Services.AuthService>(); // client

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(); // located in client



var connectionString = builder.Configuration.GetConnectionString("DefualtConnection")
            ?? throw new InvalidOperationException("No Connection String Was Found");

builder.Services.AddDbContext<ReceiptVoucherDbContext>(options =>
        options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(ReceiptVoucherDbContext).Assembly.FullName))); // Register DbContext _ Update To Uses Identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ReceiptVoucherDbContext>();


builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();    // Register IUnitOfWork

// ------ Configure Passowrd ------------
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0; // Allow repeated characters
    options.Password.RequireUppercase = false;


});

//--------------------


//--- JWT Configurations
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ClockSkew = TimeSpan.Zero

    };
});

//-------------------

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

// -----
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapControllers();



app.Run();

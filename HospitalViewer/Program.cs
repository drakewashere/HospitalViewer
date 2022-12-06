using HospitalViewer.Data;
using HospitalViewer.Data.Interfaces;
using HospitalViewer.EndpointHandlers;
using HospitalViewer.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddScoped<IHospitalService, HospitalService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html"); ;

app.MapGet("oidcConfiguration/_configuration/{clientId}", 
    (string clientId, IClientRequestParametersProvider provider, HttpContext HttpContext) => provider.GetClientParameters(HttpContext, clientId));


app.MapGet("/api/hospitals/{zip?}", HospitalEndpoints.GetHospitals);
app.MapPost("/api/hospitals/edit/", HospitalEndpoints.AddEditHospital);
app.MapDelete("/api/hospitals/delete/{hospitalId}", HospitalEndpoints.RemoveHospital);

app.MapGet("/api/contacts/{hospitalId}", ContactEndpoints.GetContactsForHospital);
app.MapPost("/api/contacts/edit", ContactEndpoints.AddEditContact);
app.MapDelete("/api/contacts/delete/{contactId}", ContactEndpoints.RemoveContact);

app.MapPost("/api/hospital/link", ContactLinkEndpoints.LinkContact);
app.MapDelete("/api/hospital/unlink/{hospitalId}/{contactId}", ContactLinkEndpoints.UnlinkContact);

app.MapGet("/api/data/generate", TestDataEndpoints.Generate);
app.MapGet("/api/data/truncate", TestDataEndpoints.Truncate);

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pattern;
using Pattern.Entity.Data;
using Pattern.Repository;
using Pattern.Repository.Interface;
using Pattern.Service;
using Pattern.Service.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add connection string
builder.Services.AddDbContext<PatternContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("PatternConnection")
    ));


//session
builder.Services.AddSession();


builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

//Repo
builder.Services.AddScoped<ISkillRepo, SkillRepo>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();

//Service
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IAccountService, AccountService>();

//Authorization middleware
builder.Services.AddScoped<AuthorizeMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseMiddleware<AuthorizeMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();

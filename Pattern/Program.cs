using Microsoft.EntityFrameworkCore;
using Pattern.Entity.Data;
using Pattern.Repository;
using Pattern.Repository.Interface;
using Pattern.Service;
using Pattern.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

//Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add connection string
builder.Services.AddDbContext<PatternContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("PatternConnection")
    ));


builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

//Repo
builder.Services.AddScoped<ISkillRepo, SkillRepo>();

//Service
builder.Services.AddScoped<ISkillService, SkillService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

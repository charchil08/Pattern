using Microsoft.EntityFrameworkCore;
using Pattern;
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

builder.Services.AddHttpContextAccessor();

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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


//Authorization middleware
builder.Services.AddScoped<AuthorizeMiddleware>();


var app = builder.Build();


app.UseExceptionHandler("/Error/GlobalException");

app.UseStatusCodePagesWithReExecute("/Error/Index", "?statusCode={0}");


app.UseSession();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<AuthorizeMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

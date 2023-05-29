using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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


builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

//Repo
builder.Services.AddScoped<ISkillRepo, SkillRepo>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();

//Service
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IAccountService, AccountService>();


//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
        ValidAudience = builder.Configuration["JwtSetting:Issuer"],
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:SecretKey"])),
    };
});

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

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();

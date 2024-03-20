using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using TaskManager.Middlewares;
using TaskManager.Models;
using TaskManager.Models.DomainModels;
using TaskManager.Models.Repositories.implementers;
using TaskManager.Models.Repositories.interfaces;
using TaskManager.Middlewares;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using TaskManager.Services.Implementers;
using TaskManager.Services.Interfaces;
using TaskManager.Configurations;
using TaskManager.Models.Models;

var builder = WebApplication.CreateBuilder(args);

// mvc configurations with json respone + NewtonsoftJson (for patial update in patch reuqest)
builder.Services.AddControllersWithViews(c=>c.Filters.Add(new AuthorizeFilter())).AddNewtonsoftJson(Options => Options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore).AddJsonOptions(Options => Options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

// authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(e=>e.LoginPath="/auth/login");

// repositories
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
builder.Services.AddScoped<ITeamsRepository, TeamsRepository>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IActivitiesRepository, ActivitiesRepository>();
builder.Services.AddScoped<IInvitesRepository, InvitesRepository>();
builder.Services.AddScoped<IResetPasswordRepository, ResetPasswordRepository>();

// configurations Objects

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Custom services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IEmailService, EmailService>();


// configurting the database(mysql)
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaskManager.Models.TaskManagerDbContext>(Options=>{
    Options.UseMySql(
        connectionString, ServerVersion.AutoDetect(connectionString)
        );
});


// swagger configurations 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  register httpContext as service
builder.Services.AddHttpContextAccessor();

// set Multipart Body Length Limit to 10MB
 builder.Services.Configure<FormOptions>(Options => Options.MultipartBodyLengthLimit = 10000000);

var app = builder.Build();

app.UseStaticFiles();

if(app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();



app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider= new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath,"Storage")),
    RequestPath="/static"
});

// middleware for iserting tasks activites in the database
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/Tasks") && !context.Request.Method.Equals("GET") && context.Response.StatusCode == 200, AppBuilder => AppBuilder.UseMiddleware<TasksActivitiesMiddleware>());


app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=dashboard}/{id?}");

// insert dummay data
if(app.Environment.IsDevelopment())

DbSeeder.Seed(app);

app.Run();
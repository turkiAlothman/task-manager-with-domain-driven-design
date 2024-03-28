using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using TaskManager.Middlewares;
using Application.Services.Interfaces;
using infrastructure;
using infrastructure.Persistence.Repositores;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using TaskManager.Services;
using infrastructure.Configurations;
using TaskManager;
using infrastructure.Persistence;
using infrastructure;
using Domain.DomainModels.Activitiy;
using Domain.DomainModels.Comment;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.DomainModels.Task;
using Domain.DomainModels.Team;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                          .AddEnvironmentVariables();

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
builder.Services.AddApplicationServices();


// configurting the database(mysql)
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaskManagerDbContext>(Options=>{
    Options.UseMySql(
        connectionString, ServerVersion.AutoDetect(connectionString) , options => options.MigrationsAssembly("TaskManager").CommandTimeout(50)
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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
    

app.UseAuthentication();


// middleware for iserting tasks activites in the database
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/Tasks") && !context.Request.Method.Equals("GET") && context.Response.StatusCode == 200, AppBuilder => AppBuilder.UseMiddleware<TasksActivitiesMiddleware>());

 app.UseExceptionHandler("/error");

app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=dashboard}/{id?}");

// insert dummay data
if(app.Environment.IsDevelopment())

DbSeeder.Seed(app);

app.Run();
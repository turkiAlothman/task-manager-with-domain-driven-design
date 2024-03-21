using System.Diagnostics;
using TaskManager.HttpExtensions;
using System.Text.Json;
using Azure;
using Newtonsoft.Json.Linq;
using Domain.Models.Repositories.interfaces;
using System.Linq;
using Domain.Entities;

namespace TaskManager.Middlewares
{
    public class TasksActivitiesMiddleware
    {

        private readonly RequestDelegate _next;

        public TasksActivitiesMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext Context, IEmployeesRepository _employeesRepository, ITasksRepository _tasksRepository, IActivitiesRepository _activitiesRepository,IProjectsRepository _projectsRepository)
        {

            Context.Request.EnableBuffering();
            await _next(Context);
            Context.Request.Body.Position = 0;
            
            int actorId = Context.User.GetUserId();
            //Debug.WriteLine(Context.Request.Path.ToString());
            //Debug.WriteLine((Context.Request.Path.ToString().Split("/").Length - 1) == 3 && Context.Request.Path.StartsWithSegments("/api/Tasks"));


            if (Context.Response.StatusCode == 200)
            {
                

                if (Context.Request.Method.Equals("PATCH"))
                {
                
                if (Context.Request.Path.StartsWithSegments("/api/tasks/addAsignee") || Context.Request.Path.StartsWithSegments("/api/tasks/RemoveAsignee"))
                {
                    int TaskId = int.Parse(Context.Request.RouteValues.GetValueOrDefault("TaskId").ToString());
                    int asigneeID = int.Parse(Context.Request.RouteValues.GetValueOrDefault("asigneeID").ToString());
                    
                    string action = (Context.Request.Path.StartsWithSegments("/api/tasks/addAsignee")) ? "Assigned" : "Removed";

                    Employees assigne = await _employeesRepository.GetEmployee(asigneeID);
                    Employees actor = await _employeesRepository.GetEmployee(actorId);
                    Tasks task =  await _tasksRepository.GetTaskWithProject(TaskId);

                    _activitiesRepository.AddChangeStatusActivities([new Activities 
                    {
                        task = task,
                        actor = actor,
                        description=$"{action} {assigne.FirstName} {assigne.LastName}",
                        ProjectName=task.Project.Name
                    }
                    ]);
                    
                }
                else if(Context.Request.Path.StartsWithSegments("/api/Tasks") && (Context.Request.Path.ToString().Split("/").Length - 1) == 3)
                {

                    int TaskID = int.Parse(Context.Request.RouteValues.GetValueOrDefault("TaskId").ToString());

                    Tasks task = await _tasksRepository.GetTaskWithProject(TaskID);
                    Employees actor = await _employeesRepository.GetEmployee(actorId);


                    JArray changes = JArray.Parse(await Context.Request.Body.ReadAsStringAsync());
                    List<Activities> activities = new List<Activities>();
                    foreach (var item in changes)
                    {
                        Debug.WriteLine(item["path"]);
                        string key = item["path"].ToString().Substring(1);
                        string Value = item["value"].ToString();

                        activities.Add(new Activities
                        {
                            task = task,
                            actor = actor,
                            description = $"chnaged {key} to {Value}",
                            ProjectName = task.Project.Name
                        });
                    }
                    _activitiesRepository.AddChangeStatusActivities(activities);



                }

                }

                else if (Context.Request.Method.Equals("POST"))
                {
                    string BodyString = await Context.Request.Body.ReadAsStringAsync();
                    

                    
                        Employees actor = await _employeesRepository.GetEmployee(actorId);
                        
                    Projects project = await _projectsRepository.GetById( int.Parse(Context.Request.Form["project"].ToString()) );
                    ;

                        _activitiesRepository.AddChangeStatusActivities([new Activities
                        {
                            actor = actor,
                            description = $"created new task <span class=\"fw-bold\">{Context.Request.Form["title"]}</span>",
                            ProjectName = project.Name

                        }]);
                    

                }
                else if (Context.Request.Method.Equals("DELETE"))
                {

                }
            

                }

            

        }
    }
}

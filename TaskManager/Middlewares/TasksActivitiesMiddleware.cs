using Domain.Activitiy;
using Domain.DomainModels.Activitiy;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.DomainModels.Task;
using Domain.Employee;
using Domain.Project;
using Domain.Task;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using TaskManager.HttpExtensions;

namespace TaskManager.Middlewares
{
    public class TasksActivitiesMiddleware
    {

        private readonly RequestDelegate _next;

        public TasksActivitiesMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext Context, IEmployeesRepository _employeesRepository, ITasksRepository _tasksRepository, IActivitiesRepository _activitiesRepository, IProjectsRepository _projectsRepository)
        {


            // allow Buffering to be able to parse the request body many time(in the controller middleware and here)
            Context.Request.EnableBuffering();

            await _next(Context);

            // restart the index 
            Context.Request.Body.Position = 0;

            int actorId = Context.User.GetUserId();

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
                        Tasks task = await _tasksRepository.GetTaskWithProject(TaskId);

                        Activities activity = new Activities($"{action} {assigne.FirstName} {assigne.LastName}", task.Project.Name);
                        activity.SetTask(task);
                        activity.SetActor(actor);

                        _activitiesRepository.AddChangeStatusActivities([activity]);

                    }
                    else if (Context.Request.Path.StartsWithSegments("/api/Tasks") && (Context.Request.Path.ToString().Split("/").Length - 1) == 3)
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

                            Activities activity = new Activities($"chnaged {key} to {Value}", task.Project.Name);
                            activity.SetTask(task);
                            activity.SetActor(actor);
                            activities.Add(activity);
                        }
                        _activitiesRepository.AddChangeStatusActivities(activities);



                    }

                }

                else if (Context.Request.Method.Equals("POST"))
                {
                    string BodyString = await Context.Request.Body.ReadAsStringAsync();



                    Employees actor = await _employeesRepository.GetEmployee(actorId);

                    Projects project = await _projectsRepository.GetById(int.Parse(Context.Request.Form["project"].ToString()));
                    ;

                    Activities activity = new Activities($"created new task <span class=\"fw-bold\">{Context.Request.Form["title"]}</span>", project.Name);
                    activity.SetActor(actor);

                    _activitiesRepository.AddChangeStatusActivities([activity]);


                }
                else if (Context.Request.Method.Equals("DELETE"))
                {


                }


            }



        }
    }
}

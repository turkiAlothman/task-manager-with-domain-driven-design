using Domain.Activitiy;
using Domain.Base;
using Domain.Comment;
using Domain.Employee;
using Domain.Project;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.JsonPatch;
using Domain.DomainModels.Task;

namespace Domain.Task
{
    public partial class Tasks : IAggregateRoot
    {
        public Tasks(string? Title, DateTime StartDate, DateTime DueDate, string? Priority , string? Description, string? Status, string? Type )
        {
            this.Title = Title;
            this.StartDate = StartDate;
            this.DueDate = DueDate;
            this.Type = Type;
            this.Priority = Priority;
            this.Description = Description;
            this.Status = Status;
        }
        public Tasks() { } 
        public void SetTitle(string title)
        {
            this.Title = title;
        }
        public void SetDescription(string description)
        {
            this.Description = description;
        }
        public void SetStatus(string status)
        {
            this.Status = status;
        }
        public void SetPriority(string priority)
        {
            this.Priority = priority;
        }
        public void SetReporter(Employees employees)
        {
            Reporter = employees;
        }
        public void SetProject(Projects project)
        {
            this.Project = project;
        }
        public void AddComment(Comments comment)
        {
            this.Comments.Add(comment);
        }
        public void AddActiviy(Activities activity)
        {
            this.Activities.Add(activity);
        }
        public void AddAssignee(Employees employee)
        {
            this.Asignees.Add(employee);
        }
        public void AddAssignees(List<Employees> employees)
        {
            ((List<Employees>)(this.Asignees)).AddRange(employees.Select(e => e));
        }
        public void AddAttachments(List<Attachments> attachments)
        {

            ((List<Attachments>)(this.Attachments)).AddRange(attachments.Select(e => e));
        }
        public static Tasks createDummy(dynamic fake)
        {
            Type type = fake.GetType();
            
            Tasks task = new Tasks();
            task.Title = fake.Title;
            task.StartDate = fake.Status;
            task.DueDate = fake.DueDate;
            task.Description = fake.Description;
            task.Status = fake.Status;
            task.Type = fake.Type;
            task.Priority = fake.Priority;
            task.Project = fake.Project;
            task.Reporter = fake.Reporter;
            task.Asignees = new List<Employees>() { fake.Asignees };

            return task;

        }
    }
}

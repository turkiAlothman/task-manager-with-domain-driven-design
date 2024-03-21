

using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Entities;
using TaskManager.RequestForms;
namespace TaskManager.Components
{
    public class CreateProjectModal : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new CreateProjectForm());
        }
        
    }
}

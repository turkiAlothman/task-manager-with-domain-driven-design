

using Microsoft.AspNetCore.Mvc;
using TaskManager.Models.DomainModels;
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

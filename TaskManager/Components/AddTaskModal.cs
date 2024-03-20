using Microsoft.AspNetCore.Mvc;
using TaskManager.RequestForms;

namespace TaskManager.Components
{
    public class AddTaskModal : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            
            return View(new AddTaskForm());
        }
    }
}

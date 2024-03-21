using Microsoft.AspNetCore.Mvc;
using TaskManager.Components.ViewModels;
using Domain.Entities;


namespace TaskManager.Components
{
    public class AddAsigneesCard : ViewComponent
    {

        public IViewComponentResult Invoke(bool allowChange, List<Employees> Asignees , Tasks task , bool TaskAdded , bool Execlude)
        {
            AddAsigneesCardModel data = new AddAsigneesCardModel(allowChange,Asignees,task,TaskAdded, Execlude);
            return View(data);
        }
    }
}

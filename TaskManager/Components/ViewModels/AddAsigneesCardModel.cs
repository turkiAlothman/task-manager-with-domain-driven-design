using Domain.Employee;
using Domain.Task;

namespace TaskManager.Components.ViewModels
{
    public class AddAsigneesCardModel
    {
        public bool allowChange { get; set; }
        public List<Employees> Asignees { get; set; } = new List<Employees>();
        public Tasks task { get; set; }
        public bool TaskAdded { get; set; }
        public bool Execlude { get; set; }

        
        public AddAsigneesCardModel(bool allowChange , List<Employees> Asignees,Tasks task , bool TaskAdded,bool Execlude) {
        
        this.Asignees = Asignees;
        this.allowChange = allowChange;
        this.task = task;
        this.TaskAdded = TaskAdded;
        this.Execlude = Execlude;
        }

    }
}

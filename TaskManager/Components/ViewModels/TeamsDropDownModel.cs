using TaskManager.Models.DomainModels;

namespace TaskManager.Components.ViewModels
{
    public class TeamsDropDownModel
    {
        public  IEnumerable<Teams> teams { get; set; }
        public Teams SelectedTeam { get; set; }

        public TeamsDropDownModel(IEnumerable<Teams> teams , Teams SelectedTeam) {
        this.teams = teams;
        this.SelectedTeam = SelectedTeam;
        }
    }
}

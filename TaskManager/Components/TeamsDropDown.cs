using Microsoft.AspNetCore.Mvc;
using infrastructure.Persistence;

using Domain.Models.Repositories.interfaces;
using TaskManager.Components.ViewModels;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;
using Domain.Team;

namespace TaskManager.Components
{
    public class TeamsDropDown : ViewComponent
    {
        private readonly ITeamsRepository _teamsRepository;
        public TeamsDropDown(ITeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? TeamId = null)
        {
            IEnumerable<Teams> teams = await _teamsRepository.GetAll();

            Teams SelectedTeam = new Teams();
            if (!TeamId.IsNullOrEmpty())
                SelectedTeam = teams.FirstOrDefault(t=>t.Id == int.Parse(TeamId));

            return View(new TeamsDropDownModel(teams, SelectedTeam));
        }
    }
}

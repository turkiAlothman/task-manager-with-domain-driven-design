﻿using Microsoft.AspNetCore.Mvc;
using Domain.Models.Repositories.interfaces;
using Domain.Team;

namespace TaskManager.Components
{
    public class OptionalTeamsDropDown : ViewComponent
    {
        private readonly ITeamsRepository _teamsRepository;

        public OptionalTeamsDropDown(ITeamsRepository teamsRepository)
        {
            this._teamsRepository = teamsRepository;
        }
    
        public  async  Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Teams> teams = await _teamsRepository.GetAll();
            return View(teams); 
        }
    }
}

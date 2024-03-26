using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Components.ViewModels;
using Domain.Project;
using Domain.DomainModels.ResetPasswords;
namespace TaskManager.Components
{
    public class ProjectsDropDown : ViewComponent
    {
        private readonly IProjectsRepository _projectsRepository;
        public ProjectsDropDown(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? ProjectID = null)
        {
            IEnumerable<Projects> projects = await _projectsRepository.GetAll();
            
            Projects Selected  = new Projects();
            if (!projects.IsNullOrEmpty())
            {
                Selected = projects.FirstOrDefault();
            }
            return View( new ProjectsDropDownModel(projects , Selected));
        }
    }
}

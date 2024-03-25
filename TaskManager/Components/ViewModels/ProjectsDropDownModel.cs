using Domain.Project;

namespace TaskManager.Components.ViewModels
{
    public class ProjectsDropDownModel
    {
        public IEnumerable<Projects> projects {get; set;}
        public Projects Selected {  get; set;}

        public ProjectsDropDownModel(IEnumerable<Projects> projects, Projects selected)
        {
            this.projects = projects;
            Selected = selected;
        }
    }
}

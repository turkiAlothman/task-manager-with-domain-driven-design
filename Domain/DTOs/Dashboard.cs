using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public record  Dashboard
    {
        public int TasksCount { get; set; }
        public int TeamsCount { get; set; }
        public int ProjectsCount { get; set; }
        public int ActivitiesCount { get; set; }

        public List<TeamStatus> TeamStatuses { get; set; }
        public List<PriorityStatus> PriorityStatuses { get; set; }
        public List<TasksStatusPercentage> TasksStatusesPercentage { get; set;}
    }
}

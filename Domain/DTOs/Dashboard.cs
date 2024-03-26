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
    }
}

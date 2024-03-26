using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class TeamStatus
    {
        public string TeamName { get; set; }
        public int tasksCount { get; set; }
        public int tasksCompletedCount { get; set; }
    }
}

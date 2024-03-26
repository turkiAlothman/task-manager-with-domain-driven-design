using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public record TasksStatusPercentage
    {
        public string StatusName { get; set; }
        public int StatusCount { get; set; }
    }
}

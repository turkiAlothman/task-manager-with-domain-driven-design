using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public record PriorityStatus
    {
        public int PriorityCount { get; set;}
        public string PriorityName { get; set; }
    }
}

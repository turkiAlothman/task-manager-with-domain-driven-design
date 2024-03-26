using Domain.Task;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Extentions
{
    public static class JsonPatch
    {
        public static void ApplyToTask(this  JsonPatchDocument<Tasks> patch , Tasks task)
        {
            foreach(var operation in patch.Operations)
            {
                if (operation.path.Equals("/title", comparisonType: StringComparison.OrdinalIgnoreCase))
                    task.SetTitle(operation.value.ToString());

                else if (operation.path.Equals("/description", comparisonType: StringComparison.OrdinalIgnoreCase))
                    task.SetDescription(operation.value.ToString());

                else if (operation.path.Equals("/status", comparisonType: StringComparison.OrdinalIgnoreCase))
                    task.SetStatus(operation.value.ToString());


                else if (operation.path.Equals("/priority", comparisonType: StringComparison.OrdinalIgnoreCase))
                    task.SetPriority(operation.value.ToString());



            }
        }
    }
}

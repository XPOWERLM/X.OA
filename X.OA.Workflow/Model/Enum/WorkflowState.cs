using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.OA.Workflow.Model
{
    public enum WorkflowState : int
    {
        Initial,
        Grant,
        Denied,
        Continue,
        Transfer,
        Restart,
        End
    }
}

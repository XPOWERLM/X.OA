using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.OA.Workflow
{
    public class ResumeBookMarkModel<T>
    {
        public int StepId { get; set; }
        public string BookMarkName { get; set; }
        
        public T Result { get; set; }
    }
}

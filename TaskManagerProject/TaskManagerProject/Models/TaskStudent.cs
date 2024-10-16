using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerProject.Models
{
    internal class TaskStudent
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set;} 
        public int isCompleted { get; set;} 
        public int SubjectId { get; set; }

        public int isExpired { get; set;}
        public string dateLine { get; set; }

        public int WorkId { get; set; }
    }
}

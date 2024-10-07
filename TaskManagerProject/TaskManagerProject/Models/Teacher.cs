using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerProject.Models
{
    internal class Teacher: User
    {
        public Teacher(int id, string firstName, string lastName, string thirdName, string userType, string password): base(id, firstName, lastName, thirdName, userType, password)
        {

        }
    }
}

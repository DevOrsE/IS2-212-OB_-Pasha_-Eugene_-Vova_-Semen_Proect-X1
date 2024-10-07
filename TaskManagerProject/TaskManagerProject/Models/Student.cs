using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerProject.Repository;

namespace TaskManagerProject.Models
{
    internal class Student: User
    {
        public Student(int id, string firstName, string lastName, string thirdName, string userType, string password) : base(id, firstName, lastName, thirdName, userType, password)
        {

        }

        public bool isStarosta { get; set; }

        public async Task<bool> SetStarosta()
        {
            SQLManager sQLManager = new SQLManager();
            bool isChanged = await sQLManager.SetStarosta(id);
            isStarosta = isChanged;
            return isChanged;
        }

        public async Task<bool> UnSetStarosta()
        {
            SQLManager sQLManager = new SQLManager();
            bool isChanged = await sQLManager.UnSetStarosta(id);
            isStarosta = isChanged;
            return isChanged;
        }

    }
}

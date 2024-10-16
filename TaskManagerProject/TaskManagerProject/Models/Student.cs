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
        public Student(int id, string firstName, string lastName, string thirdName, string userType, string password, string studentGroup, string phoneNumber, int grade) : base(id, firstName, lastName, thirdName, userType, password)
        {
            StudentGroup = studentGroup;
            PhoneNumber = phoneNumber;
            Grade = grade;
        }

        public bool isStarosta { get; set; }

        public string StudentGroup { get; set; }

        public string PhoneNumber { get; set; }

        public int Grade { get; set; }

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

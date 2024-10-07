using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerProject.Models
{
    internal class User
    {

        public int id;
        public string firstName, lastName, thirdName, userType, password;

        public User(int id, string firstName, string lastName, string thirdName, string userType, string password) { 
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.thirdName = thirdName;
            this.userType = userType;
            this.password = password;
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }


        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }


        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }


        public string ThirdName
        {

            get
            {
                return thirdName;
            }
            set
            {
                thirdName = value;
            }
        }

        public string UserType
        {
            get
            {
                return userType;
            }
            set
            {
                userType = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
    }
}

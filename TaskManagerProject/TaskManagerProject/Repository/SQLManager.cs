using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagerProject.Models;

namespace TaskManagerProject.Repository
{

    internal class SQLManager
    {
        private string connectionString = "Server=WIN-BSADAD78UM3\\MSSQLSERVER01;Database=TaskManager;Trusted_Connection=True;";
        private SqlConnection connection;

        public SQLManager()
        {
            try
            {
                connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int amountOfTables = 2;

        public async Task<bool> SetStarosta(int id)
        {
            bool isChanged = false;
            string sqlException = $"UPDATE StudentNew SET isStarosta = {1} WHERE ID = @ID ";
            SqlCommand sqlCommand = new SqlCommand(sqlException, connection);
            sqlCommand.Parameters.AddWithValue("ID", id);
            await connection.OpenAsync();
            int number = await sqlCommand.ExecuteNonQueryAsync();
            if (number == 0) isChanged = false;
            if (number == 1) isChanged = true;
            connection.Close();
            return isChanged;
        }

        public async Task<bool> UnSetStarosta(int id)
        {
            bool isChanged = false;
            string sqlException = $"UPDATE StudentNew SET isStarosta = {0} WHERE ID = @ID ";
            SqlCommand sqlCommand = new SqlCommand(sqlException, connection);
            sqlCommand.Parameters.AddWithValue("ID", id);
            await connection.OpenAsync();
            int number = await sqlCommand.ExecuteNonQueryAsync();
            if (number == 0) isChanged = false;
            if (number == 1) isChanged = true;
            connection.Close();
            return isChanged;
        }

        public async Task<bool> RegisterUser(string firstName, string lastName, string thirdName, string password, string PhoneNumber, string StudentGroup, bool isExist)
        {
            int isStarosta = 0;
            string usertype = "Student";
            string SqlExpression = "INSERT INTO StudentNew(FirstName, LastName, ThirdName, Password, isStarosta, StudentGroup, PhoneNumber, UserType) VALUES (@FirstName, @LastName, @ThirdName, @Password, @isStarosta, @StudentGroup, @PhoneNumber,@UserType)";

            using (SqlCommand sqlCommand = new SqlCommand(SqlExpression, connection))
            {
                sqlCommand.Parameters.AddWithValue("@FirstName", firstName);
                sqlCommand.Parameters.AddWithValue("@LastName", lastName);
                sqlCommand.Parameters.AddWithValue("@ThirdName", thirdName);
                sqlCommand.Parameters.AddWithValue("@UserType", usertype);
                sqlCommand.Parameters.AddWithValue("@Password", password);
                sqlCommand.Parameters.AddWithValue("@isStarosta", isStarosta);
                sqlCommand.Parameters.AddWithValue("@StudentGroup", StudentGroup);
                sqlCommand.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);


                await connection.OpenAsync();

                if (!isExist)
                {
                    int num = await sqlCommand.ExecuteNonQueryAsync();
                    connection.Close();
                    return true;
                }
                else
                {
                    connection.Close();
                    return false;
                }

            }
        }

        public async Task<List<Group>> GetAllGroups()
        {
            string SqlExpression = "SELECT * FROM Groups";
            SqlCommand sqlCommand = new SqlCommand(SqlExpression, connection);
            List<Group> collectedGroups = new List<Group>();
            await connection.OpenAsync();
            using(var reader = await sqlCommand.ExecuteReaderAsync()) // запрос выполняется 
            {
                // получены данные <-
                while(await reader.ReadAsync()) // считывает каждый столбец и записывает в список
                {
                    Group group = new Group() { Id = Convert.ToInt32(reader.GetValue(0)), GroupName = Convert.ToString(reader.GetValue(1)) };
                    collectedGroups.Add(group);
                }
            }
            connection.Close();
            return collectedGroups;
        }

        public async Task<bool> CheckUserIfExist(string firstName, string lastName, string thirdName, string password)
        {
            string SqlExpression1 = "Select * From StudentNew Where FirstName = @FirstName and LastName = @LastName and ThirdName = @ThirdName and Password = @Password";
            string SqlExpression2 = "Select * From Teacher Where FirstName = @FirstName and LastName = @LastName and ThirdName = @ThirdName and Password = @Password";
            SqlCommand command1 = new SqlCommand(SqlExpression1, connection);
            SqlCommand command2 = new SqlCommand(SqlExpression2, connection);

            // Поиск в Student таблице
            command1.Parameters.AddWithValue("FirstName", firstName);
            command1.Parameters.AddWithValue("LastName", lastName);
            command1.Parameters.AddWithValue("ThirdName", thirdName);
            command1.Parameters.AddWithValue("Password", password);

            // Поиск в Teacher таблице
            command2.Parameters.AddWithValue("FirstName", firstName);
            command2.Parameters.AddWithValue("LastName", lastName);
            command2.Parameters.AddWithValue("ThirdName", thirdName);
            command2.Parameters.AddWithValue("Password", password);

            bool isExist = false;
            await connection.OpenAsync();


            using (var reader = await command1.ExecuteReaderAsync()) // запускам поиск
            {
                if (await reader.ReadAsync()) // считываем данные
                {
                    isExist = true;
                    return isExist;
                }
                else
                {
                    reader.Close();
                    using (var reader2 = await command2.ExecuteReaderAsync()) // находит объект
                    {
                        if (await reader2.ReadAsync()) // проверяет есть ли данные в этом объекте
                        {
                            isExist = true;
                            return isExist;
                        }
                        else
                        {
                            isExist = false;
                        }
                    }
                }
            }
            connection.Close();
            return isExist;
        }

    }
}

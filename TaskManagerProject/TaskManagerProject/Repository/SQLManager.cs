using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagerProject.Models;
using TaskManagerProject.MutableValues;

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

        public async Task<bool> RegisterUser(string firstName, string lastName, string thirdName, string password, string PhoneNumber, string StudentGroup, bool isExist, int Grade)
        {
            int isStarosta = 0;
            string usertype = "Student";
            string SqlExpression = "INSERT INTO StudentNew(FirstName, LastName, ThirdName, Password, isStarosta, StudentGroup, PhoneNumber, UserType, Grade) VALUES (@FirstName, @LastName, @ThirdName, @Password, @isStarosta, @StudentGroup, @PhoneNumber,@UserType, @Grade)";

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
                sqlCommand.Parameters.AddWithValue("@Grade", Grade);


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

        public async Task<Student> GetStudent(string firstName, string lastName, string thirdName)
        {
            string SqlExpression = "Select * From StudentNew Where FirstName = @FirstName and LastName = @LastName and ThirdName = @ThirdName";
            Student student = null;
            SqlCommand sqlCommand = new SqlCommand(SqlExpression, connection);
            sqlCommand.Parameters.AddWithValue("FirstName", firstName);
            sqlCommand.Parameters.AddWithValue("LastName", lastName);
            sqlCommand.Parameters.AddWithValue("ThirdName", thirdName);

            await connection.OpenAsync();

            using (var reader = await sqlCommand.ExecuteReaderAsync()) // запрос выполняется 
            {
                if (await reader.ReadAsync()) // считывает каждый столбец и записывает в список
                {
                    student = new Student(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(), reader.GetValue(2).ToString(),
                        reader.GetValue(3).ToString(), reader.GetValue(8).ToString(), reader.GetValue(4).ToString(),
                        reader.GetValue(6).ToString(), reader.GetValue(7).ToString(),
                        Convert.ToInt32(reader.GetValue(9)));
                    if (Convert.ToInt32(reader.GetValue(5)) == 1)
                    {
                        student.isStarosta = true;
                    }
                    else
                    {
                        student.isStarosta = false;
                    }
                }
            }
            connection.Close();
            return student;
        }

        public async Task<int> CheckIfStarosta(string firstName, string lastName, string thirdName)
        {
            int isStarosta = 0;

            string SqlExpression = "Select * From StudentNew Where FirstName = @FirstName and LastName = @LastName and ThirdName = @ThirdName";

            using (SqlCommand sqlCommand = new SqlCommand(SqlExpression, connection))
            {
                sqlCommand.Parameters.AddWithValue("FirstName", firstName);
                sqlCommand.Parameters.AddWithValue("LastName", lastName);
                sqlCommand.Parameters.AddWithValue("ThirdName", thirdName);

                await connection.OpenAsync();

                using (var reader = await sqlCommand.ExecuteReaderAsync()) // запрос выполняется 
                {
                    while (await reader.ReadAsync()) // считывает каждый столбец и записывает в список
                    {
                        isStarosta = Convert.ToInt32(reader.GetValue(5));
                    }
                }
               
            }
            connection.Close();
            return isStarosta;
        }

        public async Task CompleteTask(string taskName)
        {
            string sqlExpression = "UPDATE Tasks SET isCompleted = 1 Where TaskName = @TaskName";
            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);
            sqlCommand.Parameters.AddWithValue("TaskName", taskName);
            await connection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();

            connection.Close();
        }

        public async Task CreateNewTask(string taskName, string taskDescription, DateTime deadLine, int subjectId)
        {
            bool isCompleted = false;
            int isExpired = 0;


            int workId = Values.currWorkId;

            string sqlExpression = "INSERT INTO Tasks(TaskName,TaskDescription,SubjectId,WorkId,isCompleted,isExpired,deadLine) Values (@TaskName,@TaskDescription,@SubjectId,@WorkId,@isCompleted,@isExpired,@deadLine)";
            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);
            sqlCommand.Parameters.AddWithValue("TaskName", taskName);
            sqlCommand.Parameters.AddWithValue("TaskDescription", taskDescription);
            sqlCommand.Parameters.AddWithValue("deadLine", deadLine);
            sqlCommand.Parameters.AddWithValue("SubjectId", subjectId);
            sqlCommand.Parameters.AddWithValue("WorkId", workId);
            sqlCommand.Parameters.AddWithValue("isCompleted", isCompleted);
            sqlCommand.Parameters.AddWithValue("isExpired", isExpired);
            await connection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();

            connection.Close();
        }

        public async Task UnCompleteTask(string taskName)
        {
            string sqlExpression = "UPDATE Tasks SET isCompleted = 0 Where TaskName = @TaskName";
            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);
            sqlCommand.Parameters.AddWithValue("TaskName", taskName);
            await connection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();

            connection.Close();
        }

        public async ValueTask<int> GetSubjectId(string subjectName)
        {
            string SqlExpression = "SELECT * FROM Subjects Where SubjectName = @SubjectName";
            SqlCommand sqlCommand = new SqlCommand(SqlExpression, connection);
            sqlCommand.Parameters.AddWithValue("SubjectName", subjectName);

            TaskStudent task = new TaskStudent();
            await connection.OpenAsync();
            using (var reader = await sqlCommand.ExecuteReaderAsync()) // запрос выполняется 
            {
                // получены данные <-
                while (await reader.ReadAsync()) // считывает каждый столбец и записывает в список
                {
                    task = new TaskStudent() { Id = Convert.ToInt32(reader.GetValue(0)), TaskName = Convert.ToString(reader.GetValue(1)), TaskDescription = reader.GetValue(2).ToString()};
                }
            }
            connection.Close();
            return task.Id;
        }

        public async ValueTask<int> GetWorkId(string workName)
        {
            string SqlExpression = "SELECT * FROM Works Where WorkName = @WorkName";
            SqlCommand sqlCommand = new SqlCommand(SqlExpression, connection);
            sqlCommand.Parameters.AddWithValue("WorkName", workName);

            Work work = new Work();
            await connection.OpenAsync();
            using (var reader = await sqlCommand.ExecuteReaderAsync()) // запрос выполняется 
            {
                // получены данные <-
                while (await reader.ReadAsync()) // считывает каждый столбец и записывает в список
                {
                    work = new Work() { ID = Convert.ToInt32(reader.GetValue(0)), WorkName = Convert.ToString(reader.GetValue(1)), NumberWorks = Convert.ToInt32(reader.GetValue(2))};
                }
            }
            connection.Close();
            return work.ID;
        }

        public async Task<List<TaskStudent>> GetTasksFromSubject(int subjectId, int workId)
        {
            string SqlExpression = "SELECT * FROM Tasks Where SubjectId = @SubjectId and WorkId = @WorkId";
            SqlCommand sqlCommand = new SqlCommand(SqlExpression, connection);

            sqlCommand.Parameters.AddWithValue("SubjectId", subjectId);
            sqlCommand.Parameters.AddWithValue("WorkId", workId);

            List<TaskStudent> collectedTasks = new List<TaskStudent>();
            await connection.OpenAsync();
            using (var reader = await sqlCommand.ExecuteReaderAsync()) // запрос выполняется 
            {
                // получены данные <-
                while (await reader.ReadAsync()) // считывает каждый столбец и записывает в список
                {
                    TaskStudent task = new TaskStudent() { Id = Convert.ToInt32(reader.GetValue(0)), TaskName = Convert.ToString(reader.GetValue(1)), TaskDescription = Convert.ToString(reader.GetValue(2)), isCompleted = Convert.ToInt32(reader.GetValue(5)), isExpired = Convert.ToInt32(reader.GetValue(7)), dateLine = reader.GetValue(8).ToString() };
                    collectedTasks.Add(task);
                }
            }
            connection.Close();
            return collectedTasks;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            string SqlExpression = "SELECT * FROM Groups";
            SqlCommand sqlCommand = new SqlCommand(SqlExpression, connection);
            List<Group> collectedGroups = new List<Group>();
            await connection.OpenAsync();
            using (var reader = await sqlCommand.ExecuteReaderAsync()) // запрос выполняется 
            {
                // получены данные <-
                while (await reader.ReadAsync()) // считывает каждый столбец и записывает в список
                {
                    Group group = new Group() { Id = Convert.ToInt32(reader.GetValue(0)), GroupName = Convert.ToString(reader.GetValue(1)) };
                    collectedGroups.Add(group);
                }
            }
            connection.Close();
            return collectedGroups;
        }

        public async ValueTask<Subject> GetSubject(string subjectName)
        {
            string sqlExpression = "SELECT * FROM Subjects WHERE SubjectName = @SubjectName";
            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);
            sqlCommand.Parameters.AddWithValue("SubjectName", subjectName);

            Subject subject = new Subject();

            await connection.OpenAsync();

            using (var reader = await sqlCommand.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    subject = new Subject { ID = Convert.ToInt32(reader.GetValue(0)), SubjectName = reader.GetValue(1).ToString(), Grade = Convert.ToInt32(reader.GetValue(2)) };
                }
            }
            connection.Close();
            return subject;
        }

        public async Task<List<Subject>> GetSubjects(int grade)
        {
            string sqlExpression = "SELECT * FROM Subjects WHERE Grade = @Grade";
            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);
            sqlCommand.Parameters.AddWithValue("Grade", grade);

            List<Subject> subjects = new List<Subject>();

            await connection.OpenAsync();

            using (var reader = await sqlCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    subjects.Add(new Subject { ID = Convert.ToInt32(reader.GetValue(0)), SubjectName = reader.GetValue(1).ToString(), Grade = Convert.ToInt32(reader.GetValue(2)) });
                }
            }
            connection.Close();
            return subjects;
        }

        public async Task<List<Work>> GetWorks()
        {
            string sqlExpression = "SELECT * FROM Works";
            SqlCommand sqlCommand = new SqlCommand(sqlExpression, connection);

            List<Work> works = new List<Work>();

            await connection.OpenAsync();

            using (var reader = await sqlCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    works.Add(new Work { ID = Convert.ToInt32(reader.GetValue(0)), WorkName = reader.GetValue(1).ToString(), NumberWorks = Convert.ToInt32(reader.GetValue(2)) });
                }
            }
            connection.Close();
            return works;
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

using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagerProject.Models;

namespace TaskManagerProject.MetaData
{
    internal class JsonManager
    {
        static string jsonFilePath = Path.Combine(Application.CommonAppDataPath, "json1.json");
        public JsonManager(Student student) {
            if (!File.Exists(jsonFilePath))
            {
                string json = JsonSerializer.Serialize<Student>(student);
                File.WriteAllText(jsonFilePath, json);
            }
        }

        public static bool CheckIfFileExists()
        {
            if(File.Exists(jsonFilePath))
            {
                return true;
            }
            else { 
                return false;
            }
        }

        public static async System.Threading.Tasks.Task CreateJson(Student student)
        {
            await Task.Run(() =>
            {
                if (!File.Exists(jsonFilePath))
                {
                    string json = JsonSerializer.Serialize<Student>(student);
                    File.WriteAllText(jsonFilePath, json);
                }
            });  
        }

        public static async System.Threading.Tasks.Task PutJson(Student student)
        {
            using (FileStream stream = File.OpenWrite(jsonFilePath))
            {
                await JsonSerializer.SerializeAsync<Student>(stream, student);
            }
        }

        public static async System.Threading.Tasks.Task DeleteJson()
        {
            await Task.Run(() =>
            {
                if (File.Exists(jsonFilePath))
                {
                    File.Delete(jsonFilePath);
                }
            });
        }

        public static async ValueTask<Student> GetJson()
        {
            using (FileStream stream = File.OpenRead(jsonFilePath))
            {
                return await JsonSerializer.DeserializeAsync<Student>(stream);
            }
        }

    }
}

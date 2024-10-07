using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagerProject.MainScreen;
using TaskManagerProject.Repository;

namespace TaskManagerProject.Login_Registration.RegistrationAndLogin
{
    public partial class Login : Form
    {


        string password = "";
        string[] array;
        private void SetUp()
        {
            base.CenterToParent();
        }
        public Login()
        {
            InitializeComponent();
            SetUp();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private bool SetUpName(TextBox Name) {
            bool isSetUp = false;
            string name = Name.Text.ToString().Trim();
            array = new string[3];
            int count = 0;
            for(int i  = 0; i < name.Length; i++) {
                if (name[i] != ' ') {
                    array[count] = array[count] + name[i];
                    Debug.WriteLine(name[i].ToString());
                }
                else
                {
                    count++;
                    if (count == 3) return false;
                    Debug.WriteLine(count.ToString());
                    continue;                
                }
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null) {
                    isSetUp = false;
                    break;
                }
                else
                {
                    isSetUp = true;
                }
            }
            return isSetUp;
        }
        private void SetUpPassword(TextBox passwordTBox)
        {
            password = passwordTBox.Text.ToString().Trim();
        }

        void EnterSystem(bool isExist)
        {
            if (isExist)
            {
                MainForm mainForm = new MainForm();
                mainForm.Show();
                Login login = new Login();
                login.Visible = false;
            }
            else
            {
                MessageBox.Show("Неверно введенные данные! Попробуйте снова!");
            }
        }

        private async void CheckUser(string firstName, string lastName, string thirdName, string password)
        {
            SQLManager sqlManager = new SQLManager();
            bool isExist = await sqlManager.CheckUserIfExist(firstName, lastName, thirdName, password);
            EnterSystem(isExist);
        }

        private void CheckIfEmpyField(out bool isField)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                isField = false;
            }
            else
            {
                isField = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool isFilled = false;
            CheckIfEmpyField(out isFilled);
            if (isFilled)
            {
                bool isSetUp = SetUpName(textBox1);
                if (isSetUp)
                {
                    SetUpPassword(textBox2);
                    CheckUser(array[1], array[0], array[2], password);
                }
                else
                {
                    MessageBox.Show("Введите верный формат полей 1) ФИО, 2) Пароль");
                }
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены!");
            }
        }

        private void ФИО_Click(object sender, EventArgs e)
        {

        }

        private void Вход_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

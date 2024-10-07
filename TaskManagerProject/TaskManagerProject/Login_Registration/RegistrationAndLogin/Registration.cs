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
using TaskManagerProject.Models;
using TaskManagerProject.Repository;

namespace TaskManagerProject.Login_Registration.RegistrationAndLogin
{

    public partial class Registration : Form
    {

        List<Group> groups;
        string[] array;
        string phoneNumber;
        string password;
        SQLManager sQLManager;
        List<string> hints = new List<string>() { "79788252134", "Пупкин Василий Владимирович", "*****", "Выберете вашу группу" };
        char[] digits = new char[10] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '1' };
        List<TextBox> textBoxes;


        private async Task<List<Group>> GetAllTask()
        {
            return await sQLManager.GetAllGroups();
        }

        private void FillListByTB()
        {
            textBoxes = new List<TextBox>() { textBox4, textBox1, textBox2, textBox3 };
        }

        public async void SetGroups()
        {
            groups = await GetAllTask();
            foreach (Group g in groups)
            {
                comboBox1.Items.Add(g.GroupName);
            }
        }

        public void SetHints()
        {
            for (int i = 0; i < 4; i++)
            {
                textBoxes[i].Text = hints[i];
                textBoxes[i].ForeColor = Color.Gray;
            }
            textBox3.Text = hints[2];
            textBox3.ForeColor = Color.Gray;
            comboBox1.Text = hints[3];
            textBox3.ForeColor = Color.Gray;

        }



        public Registration()
        {
            sQLManager = new SQLManager();
            InitializeComponent(); ;
            SetGroups();
            FillListByTB();
            SetHints();

        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            for (int i = 0; i < digits.Length; i++)
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == hints[0] && textBox4.ForeColor == Color.Gray)
            {
                textBox4.Clear();
                textBox4.ForeColor = Color.Black;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.Text = hints[0];
                textBox4.ForeColor = Color.Gray;
            }
        }


        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBox1.Text == hints[1] && textBox1.ForeColor == Color.Gray)
            {
                textBox1.Clear();
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = hints[1];
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = hints[2];
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBox2.Text == hints[2] && textBox2.ForeColor == Color.Gray)
            {
                textBox2.Clear();
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = hints[2];
                textBox3.ForeColor = Color.Gray;
            }
        }

        private void textBox3_MouseUp(object sender, MouseEventArgs e)
        {
            if (textBox3.Text == hints[2] && textBox3.ForeColor == Color.Gray)
            {
                textBox3.Clear();
                textBox3.ForeColor = Color.Black;
            }
        }


        private bool setPhoneNumber(TextBox textBox)
        {
            if (textBox.Text.Length == 11 && textBox.ForeColor != Color.Gray)
            {
                phoneNumber = textBox.Text;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SetUpName(TextBox Name)
        {
            bool isSetUp = false;
            string name = Name.Text.ToString().Trim();
            array = new string[3];
            int count = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] != ' ' && Name.ForeColor != Color.Gray)
                {
                    array[count] = array[count] + name[i];
                    Debug.WriteLine(name[i].ToString());
                }
                else
                {
                    count++;
                    if (count == 3) return false;
                    continue;
                }
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
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

        private bool setStudentGroup(ComboBox combo)
        {
            bool isSetUp = false;
            if (combo != null)
            {
                for (int i = 0; i < combo.Items.Count; i++)
                {
                    if (combo.Text == groups[i].GroupName)
                    {
                        isSetUp = true;
                        break;
                    }
                }
            }
            else
            {
                isSetUp = false;
            }
            return isSetUp;
        }

        bool CheckFields(List<TextBox> textBoxes, ComboBox comboBox, out string message)
        {
            bool isOk = false;

            message = "Пользователь успешно зарегистрирован!";

            isOk = setPhoneNumber(textBoxes[0]);
            if (!isOk)
            {
                message = "Введите номер состоящий из 11 цифр, начиная с '7'";
                return false;
            }


            isOk = SetUpName(textBox1);
            if (!isOk)
            {
                message = "Введите верный формат поля.\nФ - фамилия, И - имя, О - отчество";
                return false;
            }

            SetUpPassword(textBox2);

            isOk = setStudentGroup(comboBox);
            if (!isOk)
            {
                message = "Введите существующую группу!";
                return false;
            }

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i].Text == "" || textBoxes[i].ForeColor == Color.Gray)
                {
                    message = "Все поля должны быть заполнены";
                    isOk = false;
                    return isOk;
                }
                else
                {
                    isOk = true;
                    return isOk;
                }
            }

            return isOk;
        }

        public async void Register()
        {
            string message;
            if (CheckFields(textBoxes, comboBox1, out message))
            {
                bool isExist = await sQLManager.CheckUserIfExist(array[1], array[0], array[2], password);
                if (!isExist)
                {
                    bool result = await sQLManager.RegisterUser(array[1], array[0], array[2], password, phoneNumber, comboBox1.Text,isExist);
                    if (result)
                    {
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                        base.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: Пользователь уже существует!");
                    }
                }

            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Register();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            base.Visible = false;
        }
    }
}

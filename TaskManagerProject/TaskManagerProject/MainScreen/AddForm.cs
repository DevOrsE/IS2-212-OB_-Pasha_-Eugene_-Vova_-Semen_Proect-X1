using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagerProject.Models;
using TaskManagerProject.MutableValues;
using TaskManagerProject.Repository;

namespace TaskManagerProject.MainScreen
{
    public partial class AddForm : Form
    {

        SQLManager sQLManager;
        public AddForm()
        {
            InitializeComponent();
            sQLManager = new SQLManager();
        }

        public bool CheckIfFieldsAreValid()
        {
            if (richTextBox1.Text != null && richTextBox2.Text != null && richTextBox1.Text.Length < 150 && richTextBox2.Text.Length < 300 && comboBox1.Text != "Выберите тип задания") { return true; }
            else { return false; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
        }

        private async void AddTask()
        {
            int id = await sQLManager.GetSubjectId(Values.currSubjectName);
            await sQLManager.CreateNewTask(richTextBox1.Text, richTextBox2.Text, dateTimePicker1.Value,id);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(CheckIfFieldsAreValid())
            {
                Values.currWorkId = await sQLManager.GetWorkId(comboBox1.Text);
                AddTask();
                MessageBox.Show("Задание успешно добавлено!");
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены корректно!");
            }
        }

        private async void AddForm_Load(object sender, EventArgs e)
        {
            List<Work> works = await sQLManager.GetWorks();
            for (int i = 0; i < 4; i++)
            {
                comboBox1.Items.Add(works[i].WorkName);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }
    }
}

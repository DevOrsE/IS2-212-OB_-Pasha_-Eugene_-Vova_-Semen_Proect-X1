using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManagerProject.Login_Registration.RegistrationAndLogin
{
    public partial class SecretForm : Form
    {
        public SecretForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "123")
            {
                Registration registration = new Registration();
                registration.Show();
                base.Close();
            }
            else
            {
                MessageBox.Show("Ваш код доступа неверный!");
            }
        }
    }
}

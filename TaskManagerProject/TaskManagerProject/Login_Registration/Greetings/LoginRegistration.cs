using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagerProject.Login_Registration.RegistrationAndLogin;

namespace TaskManagerProject.Login_Registration
{
    public partial class LoginRegistration : Form
    {

        private void SetUpConfugirations()
        {
            base.Width = 1100;
            base.Height = 623;
        }

        public LoginRegistration()
        {
            InitializeComponent();
            base.CenterToParent();
            SetUpConfugirations();
        }

        private void ShowLoginForm()
        {
            Login login = new Login();
            login.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Registration login = new Registration();
            login.Show();

            base.Visible = false;
        }

        private void LoginRegistration_Load(object sender, EventArgs e)
        {

        }
    }
}

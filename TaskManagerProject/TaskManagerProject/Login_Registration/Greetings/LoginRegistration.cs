using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagerProject.Login_Registration.RegistrationAndLogin;
using TaskManagerProject.MainScreen;
using TaskManagerProject.MetaData;

namespace TaskManagerProject.Login_Registration
{
    public partial class LoginRegistration : Form
    {

        TimerCallback timer;

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
            LoadProgressBar();
        }

        private void ShowLoginForm()
        {
            Login login = new Login();
            login.Show();
        }

        private void ProgressBar()
        {
            label3.Visible = true;
            for (int i =  0; i < 4; i++)
            {
                if (i != 3)
                {
                    progressBar1.Step += 25;
                    progressBar1.PerformStep();
                    Thread.Sleep(500);
                }
                else
                {
                    progressBar1.Step += 25;
                    progressBar1.PerformStep();
                }
                
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (JsonManager.CheckIfFileExists())
            {

                MainForm form = new MainForm();
                progressBar1.Visible = true;
                ProgressBar();
                form.Show();
            }
            else
            {
                Login login = new Login();

                login.Show();
            }
            

            base.Visible = false;
        }


        private void LoadProgressBar()
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Visible = false;
            label3.Visible = false;
        }

        private void LoginRegistration_Load(object sender, EventArgs e)
        {
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
}

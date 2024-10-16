using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TaskManagerProject.Login_Registration.RegistrationAndLogin;
using TaskManagerProject.MetaData;
using TaskManagerProject.Models;
using TaskManagerProject.MutableValues;
using TaskManagerProject.Repository;

namespace TaskManagerProject.MainScreen
{


    public partial class MainForm : Form
    {
        List<Work> works = new List<Work>();

        bool maxNameElement = false;

        SQLManager sQLManager;

        Label[] labels;

        public MainForm()
        {
            InitializeComponent();
            sQLManager = new SQLManager();
            LoadInfoStudent();
            groupBox1.BackColor = SystemColors.Window;
        }

        string[] Names = new string[4]
        {
            "Label1",
            "Label2",
            "Label3",
            "Label4",
        };

        private async Task<List<Work>> GetWorks()
        {
            return await sQLManager.GetWorks();
        }

        public async Task InitializeLabels()
        {
            labels = new Label[4];

            works = await GetWorks();

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label
                {
                    Name = Names[i],
                    AutoSize = true,
                    Text = $"{works[i].WorkName} / Количество работ: {works[i].NumberWorks}",
                    Location = new Point(20, 25 + (i * 25))
                };
                labels[i].MouseHover += new EventHandler(label_MouseHover);
                labels[i].MouseLeave += new EventHandler(label_Leave);
                labels[i].Click += new EventHandler(openNewWindow);
                groupBox1.Controls.Add(labels[i]);
            }
        }


        private void DrawLineMin(object sender, PaintEventArgs pargs)
        {
            Graphics g = pargs.Graphics;
            Pen pen = new Pen(Color.Black, 1);
            g.DrawLine(pen, new Point(500, 0), new Point(500, 800));
        }

        private void DrawLineHorizontal(object sender, PaintEventArgs pargs)
        {
            Graphics g = pargs.Graphics;
            Pen pen = new Pen(Color.Black, 1);
            g.DrawLine(pen, new Point(0, 55), new Point(1500, 55));
        }

        private void DrawLineMax(object sender, PaintEventArgs pargs)
        {
            Graphics g = pargs.Graphics;
            Pen pen = new Pen(Color.Black, 1);

            g.DrawLine(pen, new Point(750, 0), new Point(750, 700));
        }

        private async void SetInformationAboutWindow(bool isEdit)
        {
            Student student = await JsonManager.GetJson();
            bool maxNameElement = false;
            WorkMenu workMenu = new WorkMenu();
            int grade = student.Grade;
           
            List<Subject> subjects = await sQLManager.GetSubjects(grade);
            List<Label> labels = new List<Label>();
            int i = 1;
            int xLabel = 525;
            int formX, groupX;

            foreach (Subject subject in subjects)
            {
                Label label = new Label
                {
                    Name = subject.SubjectName,
                    Text = $"{subject.SubjectName}",
                    Location = new Point(10, 35 + (i * 25)),
                    AutoSize = true,
                };
                label.Font = new Font(new FontFamily("Microsoft Sans Serif"), 14);
                if (isEdit) label.Click += new EventHandler(setEditWindow);
                else label.Click += new EventHandler(SetTasks);
                label.MouseHover += new EventHandler(label_MouseHover);
                label.Click += new EventHandler(setCurrSelectedSubjectName);
                label.MouseLeave += new EventHandler(label_Leave);
                labels.Add(label);
                i++;
            }

            for (int k = 0; k < labels.Count; k++)
            {
                if (labels[k].Text.Length >= 47)
                {
                    maxNameElement = true;
                    break;
                }
            }

            if (maxNameElement == false)
            {
                groupX = 860; formX = 900; xLabel = 615;
            }
            else
            {
                groupX = 1165; formX = 1200; xLabel = 900;
            }

            workMenu.Size = new Size(formX, workMenu.Size.Height);

            Label labelName = new Label() { Text = "Предмет:", AutoSize = true, Location = new Point(10, 25) };
            Label labelGrade = new Label() { Text = "Кол-во работ:", AutoSize = true, Location = new Point(xLabel, 25) };


            GroupBox groupBox = new GroupBox() { Text = $"Список предметов {grade} курса", AutoSize = true, Location = new Point(10, 10), Font = new Font(new FontFamily("Microsoft Sans Serif"), 14), BackColor = SystemColors.Window, Size = new Size(groupX, 590) };

            if (maxNameElement) groupBox.Paint += new PaintEventHandler(DrawLineMax);
            else groupBox.Paint += new PaintEventHandler(DrawLineMin);

            groupBox.Paint += new PaintEventHandler(DrawLineHorizontal);
            groupBox.Controls.AddRange(new Label[2] { labelName, labelGrade });

            workMenu.Controls.Add(groupBox);
            groupBox.Controls.AddRange(labels.ToArray());
            workMenu.Show();
        }

        private void setCurrSelectedSubjectName(object sender, EventArgs args)
        {
            Label label = (Label)sender;
            Values.currSubjectName = label.Text;
        }

        private void setEditWindow(object sender, EventArgs args)
        {
            AddForm form = new AddForm();
            TasksWindow task = new TasksWindow();
            form.Show();
        }
        
        private async void SetTasks(object sender, EventArgs args)
        {
            Label label = (Label) sender;
            List<Label> labelsTasksName = new List<Label>();
            List<Label> labelsTasksDescription = new List<Label>();
            List<CheckBox> TasksCheckBoxes = new List<CheckBox>();
            List<Label> TasksDeadLines= new List<Label>();
            string subjectName = label.Text;
            int subjectId = await sQLManager.GetSubjectId(subjectName);
            List<TaskStudent> tasks = await sQLManager.GetTasksFromSubject(subjectId, Values.currWorkId);

            List<Label> navLabels = new List<Label>() { 
                new Label
                {
                    Text = $"Название:",
                    Location = new Point(10, 10),
                    AutoSize = true,
                    Font = new Font(new FontFamily("Microsoft Sans Serif"), 14, FontStyle.Bold)
                },
                new Label
                {
                    Text = $"Описание:",
                    Location = new Point(250, 10),
                    AutoSize = true,
                    Font = new Font(new FontFamily("Microsoft Sans Serif"), 14, FontStyle.Bold)
                },
                new Label
                {
                    Text = $"Срок сдачи:",
                    Location = new Point(910, 10),
                    AutoSize = true,
                    Font = new Font(new FontFamily("Microsoft Sans Serif"), 14, FontStyle.Bold)
                },
                new Label
                {
                    Text = $"Прогресс:",
                    Location = new Point(1080, 10),
                    AutoSize = true,
                    Font = new Font(new FontFamily("Microsoft Sans Serif"), 14, FontStyle.Bold)
                }
                };

            // Текст с названием задания
            for(int i = 0; i < tasks.Count; i++)
            {
                Label labelParse = new Label()
                {
                    Text = tasks[i].TaskName,
                    AutoSize = true,
                    Location = new Point(10, 40 + (i * 27))
                };
                labelParse.Font = new Font(new FontFamily("Microsoft Sans Serif"), 14);
                labelsTasksName.Add(labelParse);    
            }

            // Текст с описанием
            for (int i = 0; i < tasks.Count; i++)
            {
                Label labelParse = new Label()
                {
                    Text = tasks[i].TaskDescription,
                    AutoSize = true,
                    Location = new Point(250, 40 + (i * 27))
                };
                labelParse.Font = new Font(new FontFamily("Microsoft Sans Serif"), 14);
                labelsTasksDescription.Add(labelParse);
            }

            // чек боксы
            for (int i = 0; i < tasks.Count; i++)
            {
                bool isCompleted = false;

                if (tasks[i].isCompleted == 0) isCompleted = false;
                else isCompleted = true;
                CheckBox checkBoxParse = new CheckBox()
                {
                    Name = tasks[i].TaskName,
                    AutoSize = true,
                    Checked = isCompleted,
                    Location = new Point(1123, 46 + (i * 27))
                };
                checkBoxParse.Font = new Font(new FontFamily("Microsoft Sans Serif"), 14);
                checkBoxParse.MouseUp += new MouseEventHandler(CheckBox);
                TasksCheckBoxes.Add(checkBoxParse);
            }

            // Deadlines
            for (int i = 0; i < tasks.Count; i++)
            {
                Label labelParse = new Label()
                {
                    Name = tasks[i].TaskName,
                    AutoSize = true,
                    Text = Convert.ToDateTime(tasks[i].dateLine).Date.ToString(),
                    Location = new Point(910, 40 + (i * 27))
                };
                labelParse.Font = new Font(new FontFamily("Microsoft Sans Serif"), 14);
                TasksDeadLines.Add(labelParse);
            }

            TasksWindow tasksWindow = new TasksWindow();
            tasksWindow.Controls.AddRange(labelsTasksName.ToArray());
            tasksWindow.Controls.AddRange(navLabels.ToArray());
            tasksWindow.Controls.AddRange(labelsTasksDescription.ToArray());
            tasksWindow.Controls.AddRange(TasksCheckBoxes.ToArray());
            tasksWindow.Controls.AddRange(TasksDeadLines.ToArray());
            tasksWindow.Show();
        }

        private async void CheckBox(object sender, EventArgs args)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked == true) await sQLManager.CompleteTask(checkBox.Name);
            if (checkBox.Checked == false) await sQLManager.UnCompleteTask(checkBox.Name);
        }

        private async void openNewWindow(object sender, EventArgs args)
        {
            Label label = (Label)sender;
            switch (label.Name)
            {
                case "Label1":
                    Values.currWorkId = 1;
                    break;
                case "Label2":
                    Values.currWorkId = 2;
                    break;
                case "Label3":
                    Values.currWorkId = 3;
                    break;
                case "Label4":
                    Values.currWorkId = 4;
                    break;
                default:
                    break;
            }
            SetInformationAboutWindow(false);
        }

        private void label_Leave(object sender, EventArgs args)
        {
            Label label = (Label)sender;
            label.BackColor = SystemColors.Window;
        }

        private void label_MouseHover(object sender, EventArgs args)
        {
            Label label = (Label)sender;
            label.BackColor = Color.Gray;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Text = works[i].WorkName;
            }
        }

        public void LoadInfoStudent()
        {
            SetUpInfoAboutStudent();
        }

        public async void SetUpInfoAboutStudent()
        {
            await InitializeLabels();
            Student student = await JsonManager.GetJson();
            Values.currGrade = student.Grade;
            label1.Text = $"{student.LastName} {student.firstName} {student.thirdName}";
            if(!student.isStarosta) label2.Text = "Статус: Студент";
            else
            {
                label2.Text = "Статус: Староста";
            }
            label3.Text = student.StudentGroup;
        }

        async Task<int> CheckIfStarosta()
        {
            Student student = await JsonManager.GetJson();
            return await sQLManager.CheckIfStarosta(student.firstName,student.lastName,student.thirdName);
            
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            if (await CheckIfStarosta() == 1)
            {
                button2.Visible = true;
            }
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await JsonManager.DeleteJson();
            Login login = new Login();
            login.Show();
            base.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetInformationAboutWindow(true);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }

}


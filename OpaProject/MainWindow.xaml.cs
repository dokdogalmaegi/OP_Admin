using System;
using System.Windows;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Timers;

namespace OpaProject
{
    public class Teacher
    {
        public string email { get; set; }
        public string pw { get; set; }
    }
    public class Student
    {
        public string grade { get; set; }
        public string class_num { get; set; }
        public string num { get; set; }
        public string nm { get; set; }
        public string onlineFlag { get; set; }
        public string phone { get; set; }
        public string time { get; set; }
        public string email { get; set; }

    }
    public class insertStudent
    {
        public string email { get; set; }
        public string pw { get; set; }
        public string nm { get; set; }
        public int grade { get; set; }
        public int class_num { get; set; }
        public int num { get; set; }
        public string phone { get; set; }
    }

    public class updateStudent
    {
        public string email { get; set; }
        public string pw { get; set; }
        public string nm { get; set; }
        public int grade { get; set; }
        public int class_num { get; set; }
        public int num { get; set; }
        public string phone { get; set; }
        public string flag { get; set; }
        public string changeEmail { get; set; }
    }
    public class deleteStudent
    {
        public string email { get; set; }
    }
    public enum Grade
    {
        FirstGrade = 1,
        SecondGrade = 2,
        ThreeGrade = 3,
        AllGrade = 4
    }
    public enum ClassNum
    {
        FirstClass = 1,
        SecondClass = 2,
        ThreeClass = 3,
        FourClass = 4,
        AllClass = 5
    }
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Grade grade = Grade.FirstGrade;
        private ClassNum classNum = ClassNum.FirstClass;
        private Boolean admin = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            loginBtn.IsEnabled = false; gradeSelector.IsEnabled = false; classSelector.IsEnabled = false; EmailBox.IsEnabled = false; PwBox.IsEnabled = false; loginBtn.Content = "로그인 중";

            string url = "http://222.110.147.50:8000";

            var client = new RestClient(url);

            var req = new RestRequest("/checkTeacher", Method.POST);
            req.AddJsonBody(new { email = EmailBox.Text, pw = PwBox.Password });

            req.AddHeader("Content-Type", "application/json");

            IRestResponse res = await client.ExecuteAsync(req);
            var content = res.Content;

            var r = JObject.Parse(content);

            var check = r["check"].ToString();
            string name = "값이 없습니다.";
            int resGrade;
            int resClass;
            int resPermission;

            if (check.Equals("True"))
            {
                name = r["name"].ToString();
                resPermission = (int)r["permission"];

                if (resPermission > 0)
                {
                    if (r["grade"].ToString().Equals("") || r["class"].ToString().Equals(""))
                    {
                        resGrade = 4;
                        resClass = 5;
                        admin = true;
                    }
                    else
                    {
                        resGrade = (int)r["grade"];
                        resClass = (int)r["class"];
                    }

                    switch (resGrade)
                    {
                        case 1:
                            grade = Grade.FirstGrade;
                            break;
                        case 2:
                            grade = Grade.SecondGrade;
                            break;
                        case 3:
                            grade = Grade.ThreeGrade;
                            break;
                        default:
                            grade = Grade.AllGrade;
                            break;
                    }
                    switch (resClass)
                    {
                        case 1:
                            classNum = ClassNum.FirstClass;
                            break;
                        case 2:
                            classNum = ClassNum.SecondClass;
                            break;
                        case 3:
                            classNum = ClassNum.ThreeClass;
                            break;
                        case 4:
                            classNum = ClassNum.FourClass;
                            break;
                        default:
                            classNum = ClassNum.AllClass;
                            break;
                    }
                }
            }

            Timer timer = new Timer(1);
            timer.Elapsed += (_, e1) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if(loadingBar.Value >= 100)
                    {
                        timer.Dispose();
                        if (check.Equals("False"))
                        {
                            noticeText.Text = "실패"; loginBtn.Content = "다시 로그인"; loginBtn.IsEnabled = true; gradeSelector.IsEnabled = true; classSelector.IsEnabled = true; EmailBox.IsEnabled = true; PwBox.IsEnabled = true; loadingBar.Value = 0;
                        }
                        else
                        {
                            new MainDash(EmailBox.Text, PwBox.Password, name, grade, classNum, admin).Show();
                            Close();
                        }
                    } else
                    {
                        loadingBar.Value += 1;
                    }
                }));
            };

            timer.Start();
        }

        private void firstGrade(object sender, RoutedEventArgs e)
        {
            this.grade = Grade.FirstGrade;
        }

        private void secondGrade(object sender, RoutedEventArgs e)
        {
            this.grade = Grade.SecondGrade;
        }

        private void threeGrade(object sender, RoutedEventArgs e)
        {
            this.grade = Grade.ThreeGrade;
        }

        private void firstClassNum(object sender, RoutedEventArgs e)
        {
            this.classNum = ClassNum.FirstClass;
        }

        private void secondClassNum(object sender, RoutedEventArgs e)
        {
            this.classNum = ClassNum.SecondClass;
        }

        private void threeClassNum(object sender, RoutedEventArgs e)
        {
            this.classNum = ClassNum.ThreeClass;
        }

        private void fourClassNum(object sender, RoutedEventArgs e)
        {
            this.classNum = ClassNum.FourClass;
        }
    }
}

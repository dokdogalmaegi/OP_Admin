using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpaProject
{
    /// <summary>
    /// MainDash.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainDash : Window
    {
        private string email { get; set; }
        private string pw { get; set; }
        private string name { get; set; }
        private Grade dashGrade { get; set; }
        private ClassNum dashClass { get; set; }
        private string url = "http://222.110.147.50:8000";

        NotifyIcon notify;

        public MainDash(string email, string pw, string name, Grade dashGrade, ClassNum dashClass)
        {
            this.email = email;
            this.pw = pw;
            this.name = name;
            this.dashGrade = dashGrade;
            this.dashClass = dashClass;

            InitializeComponent();

            title.Text = "내 정보";
            mainScreen.Children.Add(new UserInfo(name, email, dashGrade, dashClass));

            Closing += MainDashClosing;

            Loaded += MainDashLoaded;
        }

        private void MainDashLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
                // 아이콘 설정부분
                notify = new System.Windows.Forms.NotifyIcon();
                notify.Icon = new System.Drawing.Icon(Path.Combine(Environment.CurrentDirectory, @"di.ico"));  // 외부아이콘 사용 시
                notify.Visible = true;
                notify.ContextMenu = menu;
                notify.Text = "디지텍 출석부";

                // 아이콘 더블클릭 이벤트 설정
                notify.DoubleClick += NotifyDoubleClick;

                System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem();
                menu.MenuItems.Add(item1);
                item1.Index = 0;
                item1.Text = "프로그램 종료";
                item1.Click += delegate (object click, EventArgs eClick)
                {
                    System.Windows.Application.Current.Shutdown();
                    notify.Dispose();
                };
            }
            catch (Exception ee)
            {

            }
        }
        private void MainDashClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        private void NotifyDoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void userInfo_Click(object sender, RoutedEventArgs e)
        {
            mainScreen.Children.Clear();
            title.Text = "내 정보";
            mainScreen.Children.Add(new UserInfo(name, email, dashGrade, dashClass));
        }
        private async void userList_Click(object sender, RoutedEventArgs e)
        {
            List<Student> studentsTrue = new List<Student>();
            List<Student> studentsFalse = new List<Student>();
            var client = new RestClient(url);

            if ((int) dashGrade < 4)
            {
                var reqF = new RestRequest("/getClassNowNotLogs/csharp", Method.POST);
                reqF.AddJsonBody(new { grade = (int)dashGrade, class_num = (int)dashClass });

                reqF.AddHeader("Content-Type", "application/json");

                IRestResponse resF = await client.ExecuteAsync(reqF);
                var contentF = resF.Content;

                var rFalse = JObject.Parse(contentF);

                var listFalse = rFalse["Students"];

                var reqT = new RestRequest("/getClassNowLogs/csharp", Method.POST);
                reqT.AddJsonBody(new { grade = (int)dashGrade, class_num = (int)dashClass });

                reqT.AddHeader("Content-Type", "application/json");

                IRestResponse resT = await client.ExecuteAsync(reqT);
                var contentT = resT.Content;

                var rTrue = JObject.Parse(contentT);

                var listTrue = rTrue["Students"];

                foreach (var student in listTrue)
                {
                    string onlineFlag = "오프라인";
                    if (student["onlineFlag"].ToString().Equals("true")) onlineFlag = "온라인";
                    studentsTrue.Add(new Student() { grade = student["grade"].ToString(), class_num = student["class"].ToString(), num = student["num"].ToString(), nm = student["nm"].ToString(), onlineFlag = onlineFlag, phone = student["phone"].ToString(), time = student["time"].ToString() });
                }

                foreach (var student in listFalse)
                {
                    string onlineFlag = "오프라인";
                    if (student["onlineFlag"].ToString().Equals("true")) onlineFlag = "온라인";
                    studentsFalse.Add(new Student() { grade = student["grade"].ToString(), class_num = student["class"].ToString(), num = student["num"].ToString(), nm = student["nm"].ToString(), onlineFlag = onlineFlag, phone = student["phone"].ToString(), time = "출석 안함" });
                }
            }
            else
            {
                var reqF = new RestRequest("/getClassNowNotLogs/csharp", Method.POST);
                reqF.AddJsonBody(new { grade = (int)dashGrade, class_num = (int)dashClass });

                reqF.AddHeader("Content-Type", "application/json");

                IRestResponse resF = await client.ExecuteAsync(reqF);
                var contentF = resF.Content;

                var rFalse = JObject.Parse(contentF);

                var listFalse = rFalse["Students"];
            }

            mainScreen.Children.Clear();
            title.Text = "출결 리스트";
            mainScreen.Children.Add(new StudentList(studentsTrue, studentsFalse));
        }
    }
}

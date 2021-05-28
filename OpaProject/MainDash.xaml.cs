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
    public class Teacher
    {
        public string email { get; set; }
        public string pw { get; set; }
    }

    public partial class MainDash : Window
    {
        
        private string email { get; set; }
        private string pw { get; set; }
        private string name { get; set; }
        private Grade dashGrade { get; set; }
        private ClassNum dashClass { get; set; }
        private int grade { get; set; }
        private int classNum { get; set; }
        private bool admin = false;
        private string url = "http://222.110.147.50:8000";
        private List<Student> reqTrue;
        private List<Student> reqFalse;

        NotifyIcon notify;
        Teacher teacher = new Teacher();

        public MainDash(string email, string pw, string name, Grade dashGrade, ClassNum dashClass, bool admin)
        {
            

            this.email = email;
            this.pw = pw;
            this.name = name;
            this.dashGrade = dashGrade;
            this.dashClass = dashClass;
            this.admin = admin;

            this.grade = (int)dashGrade;
            this.classNum = (int)dashClass;

            teacher.email = email;
            teacher.pw = pw;

            InitializeComponent();

            title.Text = "내 정보";
            mainScreen.Children.Add(new UserInfo(name, email, dashGrade, dashClass, admin));

            Closing += MainDashClosing;

            Loaded += MainDashLoaded;
        }

        private async Task<List<Student>> getStudentTrue()
        {
            List<Student> studentsTrue = new List<Student>();
            var client = new RestClient(url);
            var reqT = new RestRequest("/getNowLogs/csharp", Method.POST);

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

            return studentsTrue;
        }
        private async Task<List<Student>> getStudentFalse()
        {
            List<Student> studentsFalse = new List<Student>();
            var client = new RestClient(url);

            var reqF = new RestRequest("/getNowNotLogs/csharp", Method.POST);

            reqF.AddHeader("Content-Type", "application/json");

            IRestResponse resF = await client.ExecuteAsync(reqF);
            var contentF = resF.Content;

            var rFalse = JObject.Parse(contentF);

            var listFalse = rFalse["Students"];

            foreach (var student in listFalse)
            {
                string onlineFlag = "오프라인";
                if (student["onlineFlag"].ToString().Equals("true")) onlineFlag = "온라인";
                studentsFalse.Add(new Student() { grade = student["grade"].ToString(), class_num = student["class"].ToString(), num = student["num"].ToString(), nm = student["nm"].ToString(), onlineFlag = onlineFlag, phone = student["phone"].ToString(), time = "출석 안함" });
            }

            return studentsFalse;
        }
        private List<Student> getStudentsTrues(List<Student> trueStudents, string grade, string classNum)
        {
            var studentsTrues = trueStudents.Where(s => s.grade.Contains(grade.ToString()) && s.class_num.Contains(classNum.ToString()));
            return studentsTrues.ToList();
        }
        private List<Student> getStudentsFalses(List<Student> falseStudents, string grade, string classNum)
        {
            var studentsFalses = falseStudents.Where(s => s.grade.Contains(grade.ToString()) && s.class_num.Contains(classNum.ToString()));
            return studentsFalses.ToList();
        }
        private void MainDashLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
                // 아이콘 설정부분
                notify = new System.Windows.Forms.NotifyIcon();
                notify.Icon = new System.Drawing.Icon(Path.Combine(Environment.CurrentDirectory, @"..\\..\\di.ico"));  // 외부아이콘 사용 시
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

        private void updateStudent(List<Student> studentsTrue, List<Student> studentsFalse)
        {
            mainScreen.Children.Clear();
            mainScreen.Children.Add(new StudentList(studentsTrue, studentsFalse));
        }
        private void selectedUpdate()
        {
            List<Student> studentsTrue;
            List<Student> studentsFalse;
            if(grade != 4 && classNum != 5)
            {
                studentsTrue = getStudentsTrues(reqTrue, grade.ToString(), classNum.ToString());
                studentsFalse = getStudentsFalses (reqFalse, grade.ToString(), classNum.ToString());
            }
            else if (grade != 4)
            {
                var studentsTrues = reqTrue.Where(s => s.grade.Contains(grade.ToString()));
                var studentsFalses = reqFalse.Where(s => s.grade.Contains(grade.ToString()));

                studentsTrue = studentsTrues.ToList();
                studentsFalse = studentsFalses.ToList();
            }
            else if (classNum != 5)
            {
                var studentsTrues = reqTrue.Where(s => s.class_num.Contains(classNum.ToString()));
                var studentsFalses = reqFalse.Where(s => s.class_num.Contains(classNum.ToString()));

                studentsTrue = studentsTrues.ToList();
                studentsFalse = studentsFalses.ToList();
            }
            else
            {
                studentsTrue = reqTrue;
                studentsFalse = reqFalse;
            }
            updateStudent(studentsTrue, studentsFalse);
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
            NameBox.Visibility = Visibility.Hidden;
            selectGrid.Visibility = Visibility.Hidden;
            title.Text = "내 정보";
            mainScreen.Children.Add(new UserInfo(name, email, dashGrade, dashClass, admin));
        }

        private void userUpdate_Click(object sender, RoutedEventArgs e)
        {
            mainScreen.Children.Clear();
            NameBox.Visibility = Visibility.Hidden;
            selectGrid.Visibility = Visibility.Hidden;
            title.Text = "학생 수정하기";
            mainScreen.Children.Add(new UpdateStudent(teacher));
        }
        private void userAdd_Click(object sender, RoutedEventArgs e)
        {
            mainScreen.Children.Clear();
            NameBox.Visibility = Visibility.Hidden;
            selectGrid.Visibility = Visibility.Hidden;
            title.Text = "학생 추가하기";
            mainScreen.Children.Add(new StudentAdd(teacher));
        }
        private void userDelete_Click(object sender, RoutedEventArgs e)
        {
            title.Text = "학생 삭제하기";
            mainScreen.Children.Clear();
            mainScreen.Children.Add(new StudentDelete(teacher));
        }
        private async void userList_Click(object sender, RoutedEventArgs e)
        {
            List<Student> studentsTrue = new List<Student>();
            List<Student> studentsFalse = new List<Student>();

            reqTrue = await getStudentTrue();
            reqFalse = await getStudentFalse();

            if (admin)
            {
                NameBox.Visibility = Visibility.Visible;
                selectGrid.Visibility = Visibility.Visible;

                allGradeSel.IsSelected = true;
                allClassSel.IsSelected = true;
            }

            if ((int) dashGrade < 4)
            {
                studentsTrue = getStudentsTrues(reqTrue, grade.ToString(), classNum.ToString());
                studentsFalse = getStudentsFalses(reqFalse, grade.ToString(), classNum.ToString());
            }
            else
            {
                studentsTrue = reqTrue;
                studentsFalse = reqFalse;
            }
            title.Text = "출결 리스트";
            updateStudent(studentsTrue, studentsFalse);
        }
        private async void allGrade(object sender, RoutedEventArgs e)
        {
            this.dashGrade = Grade.AllGrade;
            grade = (int)this.dashGrade;
            selectedUpdate();
                
        }
        private void firstGrade(object sender, RoutedEventArgs e)
        {
            this.dashGrade = Grade.FirstGrade;
            grade = (int)this.dashGrade;
            selectedUpdate();
        }

        private void secondGrade(object sender, RoutedEventArgs e)
        {
            this.dashGrade = Grade.SecondGrade;
            grade = (int)this.dashGrade;
            selectedUpdate();
        }

        private void threeGrade(object sender, RoutedEventArgs e)
        {
            this.dashGrade = Grade.ThreeGrade;
            grade = (int)this.dashGrade;
            selectedUpdate();
        }
        private async void allClassNum(object sender, RoutedEventArgs e)
        {
            this.dashClass = ClassNum.AllClass;
            classNum = (int)this.dashClass;
            selectedUpdate();
        }
        private void firstClassNum(object sender, RoutedEventArgs e)
        {
            this.dashClass = ClassNum.FirstClass;
            classNum = (int)this.dashClass;
            selectedUpdate();
        }

        private void secondClassNum(object sender, RoutedEventArgs e)
        {
            this.dashClass = ClassNum.SecondClass;
            classNum = (int)this.dashClass;
            selectedUpdate();
        }

        private void threeClassNum(object sender, RoutedEventArgs e)
        {
            this.dashClass = ClassNum.ThreeClass;
            classNum = (int)this.dashClass;
            selectedUpdate();
        }

        private void fourClassNum(object sender, RoutedEventArgs e)
        {
            this.dashClass = ClassNum.FourClass;
            classNum = (int)this.dashClass;
            selectedUpdate();
        }
    }
}

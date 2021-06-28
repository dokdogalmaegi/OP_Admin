using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpaProject
{
    /// <summary>
    /// UserInsertOne.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserInsertOne : Window
    {
        public const string URL = "http://222.110.147.50:8000";
        private Grade grade { get; set; }
        private ClassNum classNum { get; set; }
        private updateStudent updateStudent;
        private Teacher teacher = new Teacher();
        private bool insertCheck = false;
        public UserInsertOne(Teacher teacher, updateStudent updateStudent)
        {
            InitializeComponent();
            if (!(updateStudent == null))
            {
                insertUpdatebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
                this.updateStudent = updateStudent;
                EmailBox.Text = updateStudent.changeEmail;
                PwBox.Text = "";
                NmBox.Text = updateStudent.nm;
                switch(updateStudent.grade)
                {
                    case 1:
                        gradeSelector.SelectedIndex = 0;
                        break;
                    case 2:
                        gradeSelector.SelectedIndex = 1;
                        break;
                    case 3:
                        gradeSelector.SelectedIndex = 2;
                        break;
                }
                switch(updateStudent.class_num)
                {
                    case 1:
                        classSelector.SelectedIndex = 0;
                        break;
                    case 2:
                        classSelector.SelectedIndex = 1;
                        break;
                    case 3:
                        classSelector.SelectedIndex = 2;
                        break;
                    case 4:
                        classSelector.SelectedIndex = 3;
                        break;
                }
                NumBox.Text = Convert.ToString(updateStudent.num);
                PhoneBox.Text = updateStudent.phone;
            }
            else insertCheck = true;
            this.teacher.email = teacher.email;
            this.teacher.pw = teacher.pw;
        }

        private async void InsertStudent(object sender, RoutedEventArgs e)
        {
            var client = new RestClient(URL);
            bool error = false;
            insertStudent student = new insertStudent();
            RestRequest req;

            if (EmailBox.Text.Length == 0)
            {
                MessageBox.Show("이메일을 입력해주세요.");
                error = true;
            }
            else student.email = EmailBox.Text;
            if (PwBox.Text.Length == 0) student.pw = "";
            else student.pw = PwBox.Text;
            if (NmBox.Text.Length == 0)
            {
                MessageBox.Show("이름을 입력해주세요.");
                error = true;
            }
            else student.nm = NmBox.Text;
            student.grade = (int)this.grade;
            student.class_num = (int)this.classNum;
            if (NumBox.Text.Length == 0)
            {
                MessageBox.Show("번호를 입력해주세요.");
                error = true;
            }
            else student.num = Convert.ToInt32(NumBox.Text);
            if (PhoneBox.Text.Length == 0) student.phone = "";
            else student.phone = PhoneBox.Text;

            if (insertCheck)
            {
                if (!error)
                {
                    req = new RestRequest("/addStudent", Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddJsonBody(new { adminEmail = teacher.email, adminKey = teacher.pw, email = student.email, pw = student.pw, nm = student.nm, grade = student.grade, class_num = student.class_num, num = student.num, phone = student.phone });

                    IRestResponse res = await client.ExecuteAsync(req);
                    var content = res.Content;

                    var r = JObject.Parse(content);

                    string msg = r["msg"].ToString();

                    string resultMsg = string.Format("{0} | {1}\n", student.email, msg);

                    MessageBox.Show(resultMsg);
                    this.Close();
                }
            }
            else
            {
                if (!error)
                {
                    req = new RestRequest("/updateStudent", Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddJsonBody(new { adminEmail = teacher.email, adminKey = teacher.pw, changeEmail = student.email, pw = student.pw, nm = student.nm, grade = student.grade, class_num = student.class_num, num = student.num, phone = student.phone, flag = updateStudent.flag, email = updateStudent.changeEmail });

                    IRestResponse res = await client.ExecuteAsync(req);
                    var content = res.Content;

                    var r = JObject.Parse(content);

                    string msg = r["msg"].ToString();

                    string resultMsg = string.Format("{0} | {1}\n", updateStudent.nm, msg);

                    MessageBox.Show(resultMsg);
                    this.Close();
                }
            }
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

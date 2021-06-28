using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OpaProject
{
    /// <summary>
    /// StudentList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StudentList : UserControl
    {
        private List<deleteStudent> selectedStudent = new List<deleteStudent>();
        private List<Student> studentsTrue;
        private List<Student> studentsFalse;
        public const string URL = "http://222.110.147.50:8000";
        private Teacher teacher { get; set; }

        public StudentList(List<Student> studentsTrue, List<Student> studentsFalse, Teacher teacher)
        {
            InitializeComponent();

            this.studentsTrue = studentsTrue;
            this.studentsFalse = studentsFalse;
            this.teacher = teacher;

            if (studentsTrue.Count() > 0) StudentsTrue.ItemsSource = studentsTrue;
            else noticeText2.Visibility = Visibility.Visible;
            if (studentsFalse.Count() > 0) StudentsFalse.ItemsSource = studentsFalse;
            else noticeText.Visibility = Visibility.Visible;

            StudentsTrue.MouseUp += StudentsTrue_MouseClick;
            StudentsTrue.MouseDoubleClick += StudentsTrue_DoubleClick;

            StudentsFalse.MouseUp += StudentsFalse_MouseClick;
            StudentsFalse.MouseDoubleClick += StudentsFalse_DoubleClick;
        }

        private void StudentsTrue_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if(StudentsTrue.SelectedIndex != -1)
            {
                // 아이템 클릭시 할 짓
                
                if (selectedStudent.Any(s => s.email == ((Student)StudentsTrue.SelectedItem).email))
                {
                    selectedStudent.Remove(selectedStudent.Find(s => s.email == ((Student)StudentsTrue.SelectedItem).email));
                    if (deleteStudents.Items.Count == 0) deleteStudents.Visibility = Visibility.Hidden;
                }
                    
                else
                {
                    selectedStudent.Add(new deleteStudent { email = ((Student)StudentsTrue.SelectedItem).email });
                    deleteStudents.Visibility = Visibility.Visible;
                }
                deleteStudents.Items.Refresh();
                deleteStudents.ItemsSource = selectedStudent;
            }
        }
        private void StudentsTrue_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StudentsTrue.SelectedIndex != -1)
            {
                // 아이템 클릭시 할 짓
                updateStudent updateStudent = new updateStudent();
                Student st = ((Student)StudentsTrue.SelectedItem);

                updateStudent.changeEmail = st.email;
                updateStudent.nm = st.nm;
                updateStudent.grade = Convert.ToInt32(st.grade);
                updateStudent.class_num = Convert.ToInt32(st.class_num);
                updateStudent.num = Convert.ToInt32(st.num);
                updateStudent.phone = st.phone;

                new UserInsertOne(teacher, updateStudent).Show();
            }
        }
        private void StudentsFalse_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (StudentsFalse.SelectedIndex != -1)
            {
                // 아이템 클릭시 할 짓
                if (selectedStudent.Any(s => s.email == ((Student)StudentsFalse.SelectedItem).email))
                {
                    selectedStudent.Remove(selectedStudent.Find(s => s.email == ((Student)StudentsFalse.SelectedItem).email));
                    if (deleteStudents.Items.Count == 0) deleteStudents.Visibility = Visibility.Hidden;
                }
                else
                {
                    selectedStudent.Add(new deleteStudent { email = ((Student)StudentsFalse.SelectedItem).email });
                    deleteStudents.Visibility = Visibility.Visible;
                }
                deleteStudents.Items.Refresh();
                deleteStudents.ItemsSource = selectedStudent;
            }
        }
        private void StudentsFalse_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StudentsFalse.SelectedIndex != -1)
            {
                // 아이템 클릭시 할 짓
                // 아이템 클릭시 할 짓
                updateStudent updateStudent = new updateStudent();
                Student st = ((Student)StudentsFalse.SelectedItem);

                updateStudent.changeEmail = st.email;
                updateStudent.nm = st.nm;
                updateStudent.grade = Convert.ToInt32(st.grade);
                updateStudent.class_num = Convert.ToInt32(st.class_num);
                updateStudent.num = Convert.ToInt32(st.num);
                updateStudent.phone = st.phone;
                updateStudent.flag = st.onlineFlag;

                new UserInsertOne(teacher, updateStudent).Show();
            }
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient(URL);
            string resultMsg = "";

            if(selectedStudent.Count == 0)
            {
                MessageBox.Show("삭제할 학생이 선택되지 않았습니다.");
            }else
            {
                foreach (deleteStudent student in selectedStudent)
                {
                    var req = new RestRequest("/deleteStudent", Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddJsonBody(new { adminEmail = teacher.email, adminKey = teacher.pw, email = student.email });

                    IRestResponse res = await client.ExecuteAsync(req);
                    var content = res.Content;

                    var r = JObject.Parse(content);

                    string msg = r["msg"].ToString();

                    resultMsg += string.Format("{0} | {1}\n", student.email, msg);
                    if (studentsTrue.Any(s => s.email.Equals(student.email))) studentsTrue.Remove(studentsTrue.Find(s => s.email.Equals(student.email)));
                    if (studentsFalse.Any(s => s.email.Equals(student.email))) studentsFalse.Remove(studentsFalse.Find(s => s.email.Equals(student.email)));
                }
                selectedStudent.Clear();

                deleteStudents.Items.Refresh();
                deleteStudents.ItemsSource = selectedStudent;
                deleteStudents.Visibility = Visibility.Hidden;

                StudentsTrue.Items.Refresh();
                StudentsTrue.ItemsSource = studentsTrue;

                StudentsFalse.Items.Refresh();
                StudentsFalse.ItemsSource = studentsFalse;

                MessageBox.Show(resultMsg, "알림 메시지");
            }
        }
        private async void PrivateInsert_Click(object sender, RoutedEventArgs e)
        {
            new UserInsertOne(teacher, null).Show();
        }
    }
}

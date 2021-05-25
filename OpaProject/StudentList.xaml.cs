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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpaProject
{
    /// <summary>
    /// StudentList.xaml에 대한 상호 작용 논리
    /// </summary>
    public class Student
    {
        public string grade { get; set; }
        public string class_num { get; set; }
        public string num { get; set; }
        public string nm { get; set; }
        public string onlineFlag { get; set; }
        public string phone { get; set; }
        public string time { get; set; }

    }
    public partial class StudentList : UserControl
    {
        public StudentList(List<Student> studentsTrue, List<Student> studentsFalse)
        {
            InitializeComponent();
            if (studentsTrue.Count() > 0) StudentsTrue.ItemsSource = studentsTrue;
            else noticeText2.Visibility = Visibility.Visible;
            if (studentsFalse.Count() > 0) StudentsFalse.ItemsSource = studentsFalse;
            else noticeText.Visibility = Visibility.Visible;
        }
    }
}

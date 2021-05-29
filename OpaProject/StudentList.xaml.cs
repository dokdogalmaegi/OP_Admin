using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OpaProject
{
    /// <summary>
    /// StudentList.xaml에 대한 상호 작용 논리
    /// </summary>
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

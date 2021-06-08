using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OpaProject
{
    /// <summary>
    /// StudentList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StudentList : UserControl
    {
        private List<deleteStudent> selectedStudent;
        public StudentList(List<Student> studentsTrue, List<Student> studentsFalse)
        {
            InitializeComponent();

            if (studentsTrue.Count() > 0) StudentsTrue.ItemsSource = studentsTrue;
            else noticeText2.Visibility = Visibility.Visible;
            if (studentsFalse.Count() > 0) StudentsFalse.ItemsSource = studentsFalse;
            else noticeText.Visibility = Visibility.Visible;

            StudentsTrue.MouseDoubleClick += StudentsTrue_MouseDoubleClick;
            StudentsFalse.MouseDoubleClick += StudentsFalse_MouseDoubleClick;
        }

        private void StudentsTrue_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(StudentsTrue.SelectedIndex != -1)
            {
                // 아이템 클릭시 할 짓
                MessageBox.Show("test");
            }
        }
        private void StudentsFalse_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StudentsTrue.SelectedIndex != -1)
            {
                // 아이템 클릭시 할 짓
            }
        }
    }
}

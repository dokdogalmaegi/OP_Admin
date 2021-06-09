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
                if (selectedStudent.Any(s => s.email == ((Student)StudentsTrue.SelectedItem).email)) selectedStudent.Remove(new deleteStudent { email = ((Student)StudentsTrue.SelectedItem).email });
                else selectedStudent.Add(new deleteStudent { email = ((Student)StudentsTrue.SelectedItem).email });
                deleteStudents.ItemsSource = selectedStudent;
                MessageBox.Show(((Student) StudentsTrue.SelectedItem).email);
            }
        }
        private void StudentsFalse_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StudentsFalse.SelectedIndex != -1)
            {
                // 아이템 클릭시 할 짓
                if (selectedStudent.Contains(new deleteStudent { email = ((Student)StudentsFalse.SelectedItem).email })) selectedStudent.Remove(new deleteStudent { email = ((Student)StudentsFalse.SelectedItem).email });
                else selectedStudent.Add(new deleteStudent { email = ((Student)StudentsFalse.SelectedItem).email });
                deleteStudents.ItemsSource = selectedStudent;
                MessageBox.Show(((Student)StudentsFalse.SelectedItem).email);
            }
        }
    }
}

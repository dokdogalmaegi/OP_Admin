using System.Windows;
using System.Windows.Controls;

namespace OpaProject
{
    /// <summary>
    /// UserInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserInfo : UserControl
    {
        public UserInfo(string name, string email, Grade grade, ClassNum classNum, bool admin)
        {
            InitializeComponent();
            emailText.Text = email;
            teacherName.Text = name + " 선생님";

            if(!admin)
            {
                switch (grade)
                {
                    case Grade.FirstGrade:
                        gradeNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number1;
                        break;
                    case Grade.SecondGrade:
                        gradeNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number2;
                        break;
                    case Grade.ThreeGrade:
                        gradeNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number3;
                        break;
                }
                switch (classNum)
                {
                    case ClassNum.FirstClass:
                        classNumNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number1;
                        break;
                    case ClassNum.SecondClass:
                        classNumNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number2;
                        break;
                    case ClassNum.ThreeClass:
                        classNumNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number3;
                        break;
                    case ClassNum.FourClass:
                        classNumNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number4;
                        break;
                }
            }
            else
            {
                GradeBox.Visibility = Visibility.Hidden; gradeNum.Visibility = Visibility.Hidden; BorderBox.Visibility = Visibility.Hidden; classNumText.Visibility = Visibility.Hidden; classNumNum.Visibility = Visibility.Hidden;
                schoolIcon.Visibility = Visibility.Hidden;
                AdminBox.Visibility = Visibility.Visible; LeftBox.Width = new GridLength(3, GridUnitType.Star);
            }
        }
    }
}

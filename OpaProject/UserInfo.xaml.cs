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
    /// UserInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserInfo : UserControl
    {
        public UserInfo(string name, string email, Grade grade, ClassNum classNum)
        {
            InitializeComponent();
            emailText.Text = email;
            teacherName.Text = name + " 선생님";
            switch(grade)
            {
                case Grade.FirstGrade :
                    gradeNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number1;
                    break;
                case Grade.SecondGrade :
                    gradeNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number2;
                    break;
                case Grade.ThreeGrade :
                    gradeNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number3;
                    break;
            }
            switch (classNum)
            {
                case ClassNum.FirstClass :
                    classNumNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number1;
                    break;
                case ClassNum.SecondClass :
                    classNumNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number2;
                    break;
                case ClassNum.ThreeClass :
                    classNumNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number3;
                    break;
                case ClassNum.FourClass :
                    classNumNum.Kind = MaterialDesignThemes.Wpf.PackIconKind.Number4;
                    break;
            }
        }
    }
}

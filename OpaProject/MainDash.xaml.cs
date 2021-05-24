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
    /// MainDash.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainDash : Window
    {
        enum Grade
        {
            FirstGrade = 1,
            SecondGrade = 2,
            ThreeGrade = 3
        }
        enum ClassNum
        {
            FirstClass = 1,
            SecondClass = 2,
            ThreeClass = 3,
            FourClass = 4
        }

        private string email { get; set; }
        private string pw { get; set; }
        private Grade grade = Grade.FirstGrade;
        private ClassNum classNum = ClassNum.FirstClass;

        public MainDash(string email, string pw)
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

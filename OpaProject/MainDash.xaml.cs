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
        private string email { get; set; }
        private string pw { get; set; }
        private Grade grade { get; set; }
        private ClassNum classNum { get; set; }

        public MainDash(string email, string pw, Grade grade, ClassNum classNum)
        {
            this.email = email;
            this.pw = pw;
            this.grade = grade;
            this.classNum = classNum;

            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void allPage_Hidden(object sender, RoutedEventArgs e)
        {

        }

        private void userInfo_Click(object sender, RoutedEventArgs e)
        {
            userInfo.Visibility = Visibility.Visible;
        }
    }
}

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
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Timers;

namespace OpaProject
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
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

        private Grade grade = Grade.FirstGrade;
        private ClassNum classNum = ClassNum.FirstClass;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            login.Visibility = Visibility.Hidden;

            string url = "http://localhost:8000";

            var client = new RestClient(url);

            var req = new RestRequest("/checkTeacher", Method.POST);
            req.AddJsonBody(new { email = EmailBox.Text, pw = PwBox.Password });

            req.AddHeader("Content-Type", "application/json");

            IRestResponse res = client.Execute(req);
            var content = res.Content;

            var r = JObject.Parse(content);

            var check = r["check"].ToString();

            Timer timer = new Timer(2000);
            timer.Elapsed += (_, e1) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    loading.Visibility = Visibility.Hidden;
                    login.Visibility = Visibility.Visible;
                    if (check.Equals("False")) noticeText.Text = check;
                    else noticeText.Text = check;
                }));
                timer.Dispose();
            };
            timer.Start();
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

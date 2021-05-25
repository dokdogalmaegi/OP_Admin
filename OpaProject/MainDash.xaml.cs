using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpaProject
{
    /// <summary>
    /// MainDash.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainDash : Window
    {
        private bool isExit = false;
        private string email { get; set; }
        private string pw { get; set; }
        private string name { get; set; }
        private Grade grade { get; set; }
        private ClassNum classNum { get; set; }

        NotifyIcon notify;

        public MainDash(string email, string pw, string name, Grade grade, ClassNum classNum)
        {
            this.email = email;
            this.pw = pw;
            this.name = name;
            this.grade = grade;
            this.classNum = classNum;

            InitializeComponent();

            title.Text = "내 정보";
            mainScreen.Children.Add(new UserInfo(name, email, grade, classNum));

            Closing += MainDashClosing;

            Loaded += MainDashLoaded;
        }

        private void MainDashLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
                // 아이콘 설정부분
                notify = new System.Windows.Forms.NotifyIcon();
                notify.Icon = new System.Drawing.Icon(Path.Combine(Environment.CurrentDirectory, @"di.ico"));  // 외부아이콘 사용 시
                notify.Visible = true;
                notify.ContextMenu = menu;
                notify.Text = "디지텍 출석부";

                // 아이콘 더블클릭 이벤트 설정
                notify.DoubleClick += NotifyDoubleClick;

                System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem();
                menu.MenuItems.Add(item1);
                item1.Index = 0;
                item1.Text = "프로그램 종료";
                item1.Click += delegate (object click, EventArgs eClick)
                {
                    System.Windows.Application.Current.Shutdown();
                    notify.Dispose();
                };
            }
            catch (Exception ee)
            {

            }
        }
        private void MainDashClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        private void NotifyDoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void userInfo_Click(object sender, RoutedEventArgs e)
        {
            mainScreen.Children.Clear();
            title.Text = "내 정보";
            mainScreen.Children.Add(new UserInfo(name, email, grade, classNum));
        }
    }
}

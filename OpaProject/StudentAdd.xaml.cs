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
using System.IO;
using Microsoft.Win32;
using OfficeOpenXml;

namespace OpaProject
{
    /// <summary>
    /// StudentAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StudentAdd : UserControl
    {
        public class insertStudent
        {
            public string email { get; set; }
            public string pw { get; set; }
            public string nm { get; set; }
            public int grade { get; set; }
            public int class_num { get; set; }
            public int num { get; set; }
            public string phone { get; set; }
        }
        public StudentAdd()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "엑셀 파일 (*.xlsx)|*.xlsx|엑셀 파일 (*.xls)|*.xls";
            if (openFileDialog.ShowDialog() == true)
            {
                var path = new FileInfo(openFileDialog.FileName);

                using (var package = new ExcelPackage(path))
                {
                    var workBook = package.Workbook;

                    // Excel 안의 AllSheet에 접근
                    var workSheet = workBook.Worksheets.FirstOrDefault();

                    int noOfRow = workSheet.Dimension.End.Row - 1;

                    int row = 2;
                    List<insertStudent> list = new List<insertStudent>();
                    for (int i = 0; i < noOfRow; i++)
                    {
                        insertStudent vo = new insertStudent();
                        vo.email = workSheet.GetValue(row, 1).ToString();
                        vo.pw = workSheet.GetValue(row, 2).ToString();
                        vo.nm = workSheet.GetValue(row, 3).ToString();
                        vo.grade = Convert.ToInt32(workSheet.GetValue(row, 4));
                        vo.class_num = Convert.ToInt32(workSheet.GetValue(row, 5));
                        vo.num = Convert.ToInt32(workSheet.GetValue(row, 6));
                        vo.phone = workSheet.GetValue(row, 7).ToString();

                        row++;

                        list.Add(vo);
                    }
                    StudentList.ItemsSource = list;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using OfficeOpenXml;
using RestSharp;
using Newtonsoft.Json.Linq;
using Path = System.IO.Path;

namespace OpaProject
{
    /// <summary>
    /// StudentAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StudentAdd : UserControl
    {
        private Teacher teacher = new Teacher();

        public const string URL = "http://222.110.147.50:8000";
        List<insertStudent> list = new List<insertStudent>();
        public StudentAdd(Teacher teacher)
        {
            this.teacher = teacher;

            InitializeComponent();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "엑셀 파일 (*.xlsx)|*.xlsx|엑셀 파일 (*.xls)|*.xls";
            if (openFileDialog.ShowDialog() == true)
            {
                list.Clear();
                var path = new FileInfo(openFileDialog.FileName);

                using (var package = new ExcelPackage(path))
                {
                    var workBook = package.Workbook;

                    // Excel 안의 AllSheet에 접근
                    var workSheet = workBook.Worksheets.FirstOrDefault();

                    int noOfRow = workSheet.Dimension.End.Row - 1;

                    int row = 2;

                    for (int i = 0; i < noOfRow; i++)
                    {
                        try
                        {
                            insertStudent vo = new insertStudent();
                            vo.email = workSheet.GetValue(row, 1).ToString();
                            if (workSheet.GetValue(row, 2) == null) vo.pw = "";
                            else vo.pw = workSheet.GetValue(row, 2).ToString();
                            vo.nm = workSheet.GetValue(row, 3).ToString();
                            vo.grade = Convert.ToInt32(workSheet.GetValue(row, 4));
                            vo.class_num = Convert.ToInt32(workSheet.GetValue(row, 5));
                            vo.num = Convert.ToInt32(workSheet.GetValue(row, 6));
                            if (workSheet.GetValue(row, 7) == null) vo.phone = "";
                            else vo.phone = workSheet.GetValue(row, 7).ToString();

                            row++;

                            list.Add(vo);
                        } catch (Exception eee)
                        {
                            Console.WriteLine(eee.Message);
                        }
                    }
                    StudentList.ItemsSource = list;
                }
            }
        }
        private void downloadBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "엑셀 파일 (*.xlsx)|*.xlsx|엑셀 파일 (*.xls)|*.xls";
            saveFileDialog.FileName = "양식.xlsx";
            saveFileDialog.InitialDirectory = @"C:\";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory, @"..\\..\\excel\form.xlsx"), FileMode.Open);
                FileStream dest = new FileStream(saveFileDialog.FileName, FileMode.Create);
                fs.CopyTo(dest);
            }
        }
        private async void insertBtn_Click(object sender, RoutedEventArgs e)
        {
            if(list.Count == 0)
            {
                MessageBox.Show("추가할 학생이 없습니다.", "알림 메시지");
            }
            else
            {
                var client = new RestClient(URL);
                string resultMsg = "";

                foreach (insertStudent student in list)
                {
                    var req = new RestRequest("/addStudent", Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddJsonBody(new { adminEmail = teacher.email, adminKey = teacher.pw, email = student.email, pw = student.pw, nm = student.nm, grade = student.grade, class_num = student.class_num, num = student.num, phone = student.phone });

                    IRestResponse res = await client.ExecuteAsync(req);
                    var content = res.Content;

                    var r = JObject.Parse(content);

                    string msg = r["msg"].ToString();

                    resultMsg += string.Format("{0} | {1}\n", student.email, msg);
                }

                MessageBox.Show(resultMsg, "알림 메시지");
            }
        }
    }
}

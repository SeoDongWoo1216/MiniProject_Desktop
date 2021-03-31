using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfSMSApp.View.User
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserList : Page
    {
        public UserList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RdoAll.IsChecked = true;
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 UserList Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddUser());
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnAddUser_Click : {ex}");
                throw ex;
            }
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new EditUser());
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnEditUser_Click : {ex}");
                throw ex;
            }
        }

        private void BtnDeactivateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new DeactiveUser());
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnDeactivateUser_Click : {ex}");
                throw ex;
            }
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF FIle(*.pdf)|*.pdf";
            saveDialog.FileName = "";
            if (saveDialog.ShowDialog() == true)
            {
                // PDF 변환
                try
                {
                    iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14);
                    string pdfFilePath = saveDialog.FileName;

                    iTextSharp.text.Document pdfDoc = new Document(PageSize.A4);

                    // 1. PDF 객체 생성
                    PdfPTable pdfTable = new PdfPTable(GrdData.Columns.Count);

                    // 2. PDF 내용 만들기
                    //string nanumttf = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), @"Fonts\NanumGothic.ttf");
                    //BaseFont nanumBase = BaseFont.CreateFont(nanumttf, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    //var nanumFont = new iTextSharp.text.Font(nanumBase, 16f);

                    //Paragraph title = new Paragraph($@"PKNU Stock Management 시스템 : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", nanumFont);

                    Paragraph title = new Paragraph($@"부경대 Stock Management System : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                    // 3. 실제 PDF 파일생성
                    using (FileStream stream = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // 2.에서 만든 내용을 추가
                        pdfDoc.Add(title);
                        pdfDoc.Close();
                        stream.Close();  // 옵션인데 일단 넣음
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외발생 BtnExportPdf_Click : {ex}");

                }
            }
        }


        private void RdoAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<WpfSMSApp.Model.User> users = new List<Model.User>();
                if (RdoAll.IsChecked == true)  // '모두' 라디오버튼 체크가 됬을때
                {
                    users = Logic.DataAccess.GetUser();
                }
                this.DataContext = users;
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
            }
        }

        private void RdoActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<WpfSMSApp.Model.User> users = new List<Model.User>();
                if (RdoActive.IsChecked == true)  // '활성' 라디오버튼 체크가 됬을때
                {
                    users = Logic.DataAccess.GetUser().Where(u => u.UserActivated == true).ToList();  // DB에서 활성화가 True인 사람만 출력
                }
                this.DataContext = users;
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
            }
        }

        private void RdoDeactive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<WpfSMSApp.Model.User> users = new List<Model.User>();
                if (RdoDeactive.IsChecked == true)  // '비활성' 라디오버튼 체크가 됬을때
                {
                    users = Logic.DataAccess.GetUser().Where(u=>u.UserActivated == false).ToList();  // DB에서 활성화가 false인 사람만 출력
                }
                this.DataContext = users;
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외발생 : {ex}");
            }
        }
    }
}

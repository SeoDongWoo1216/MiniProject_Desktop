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
                    // 0. PDF에 사용할 폰트 설정
                    string nanumPath = Path.Combine(Environment.CurrentDirectory, @"NanumGothic.ttf"); // 폰트 경로 지정
                    // Combine은 문자열 결함 함수, Environment.CurrentDirectory 현재 작업 폴더에서 프로그램 실행
                    
                    BaseFont nanumBase = BaseFont.CreateFont(nanumPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    
                    var nanumTitle = new iTextSharp.text.Font(nanumBase, 18f);  // 사이즈 20짜리 타이틀용 나눔폰트 생성

                    var nanumContent = new iTextSharp.text.Font(nanumBase, 12f); // 사이즈 12짜리 내용에쓰는 나눔폰트 생성

                   // iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14);
                    string pdfFilePath = saveDialog.FileName;
                    iTextSharp.text.Document pdfDoc = new Document(PageSize.A4);

                    
                    // 1. PDF 객체 생성
                    PdfPTable pdfTable = new PdfPTable(GrdData.Columns.Count);
                    pdfTable.WidthPercentage = 100;   // 전체 사이즈 다 쓰는거


                    // 2. PDF 내용 만들기
                    Paragraph title = new Paragraph("부경대 재고 관리 시스템(Stock Management System)\n", nanumTitle);
                    Paragraph subTitle = new Paragraph($"exported : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n\n", nanumContent);

                    // 그리드 헤더 작업
                    foreach (DataGridColumn column in GrdData.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString(), nanumContent));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cell);
                    }

                    // 각 셀 사이즈 조정
                    float[] columnsWidth = new float[] { 8f, 25f, 10f, 15f, 32f, 14f, 12f };
                    pdfTable.SetWidths(columnsWidth);

                    // 그리드 Row작업
                    foreach (var item in GrdData.Items)
                    {
                       if(item is Model.User)
                        {
                            var temp = item as Model.User;

                            // UserID
                            PdfPCell cell = new PdfPCell(new Phrase(temp.UserID.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            pdfTable.AddCell(cell);

                            // UserIdentityNumber
                            cell = new PdfPCell(new Phrase(temp.UserIdentityNumber.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            // UserSurname
                            cell = new PdfPCell(new Phrase(temp.UserSurname.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            // UserName
                            cell = new PdfPCell(new Phrase(temp.UserName.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            // UserEmail
                            cell = new PdfPCell(new Phrase(temp.UserEmail.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            // UserAdmin
                            cell = new PdfPCell(new Phrase(temp.UserAdmin.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);

                            // UserActivated
                            cell = new PdfPCell(new Phrase(temp.UserActivated.ToString(), nanumContent));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfTable.AddCell(cell);
                        }
                    }



                    // 3. 실제 PDF 파일생성
                    using (FileStream stream = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // 2.에서 만든 내용을 추가
                        pdfDoc.Add(title);
                        pdfDoc.Add(subTitle);
                        pdfDoc.Add(pdfTable);
                        pdfDoc.Close();
                        stream.Close();  // 옵션인데 일단 넣음
                    }

                    Commons.ShowMessageAsync("PDF변환", "PDF 익스포트 성공했습니다.");
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

## 프로토타입 ERP 프로젝트
- ERP는 전사적 자원 관리(Enterprise Resource Planning)의 약칭으로, 재무, 제조, 소매유통, 공급망, 인사 관리 등의 프로세스를 자동화하고 관리하는 시스템입니다.
- 본 프로젝트는 WPF의 CRUD를 학습하기위해 ERP 일부를 구현한 프로젝트입니다.
- DB 쿼리문은 Entity Framework를 활용하여 연동했습니다.
- 각 View 디자인은 .xaml 파일을 참고하시면됩니다.

--------------

## 실행화면
<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/ERP_View%EC%8B%A4%ED%96%89%ED%99%94%EB%A9%B4.gif">
</p>

<p align = "center">
(위 실행화면은 View를 보여주는 움짤입니다)
</p>

<br>

- 실행 프로그램은 로그인, 계정, 사용자, 창고 관리를 구현했습니다.
- [로그인화면](#로그인)에서 ID와 PW를 비교하여 로그인 처리하고, MD5를 통해 DB에 PW가 암호화되고, 이메일 정규식을 확인하는 메서드로 유효성 검사를 구현했습니다.
- [계정 관리](#계정관리)는 계정 정보를 열람, 수정할 수 있도록 구현했습니다.
- [사용자 관리](#사용자관리)는 추가, 수정, 계정 비활성화, PDF파일 출력으로 구현했습니다.
- [창고 관리](#창고관리)는 추가, 수정, Excel파일 출력으로 구현했습니다.

<br>

--------------

## 소스 분석

### 로그인
<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/Login.PNG">
</p>


##### - 로그인 버튼 클릭 이벤트

```C#
using System.Security.Cryptography;  // MD5 를 사용하기위한 라이브러리

private async void BtnLogin_Click(object sender, RoutedEventArgs e)
{
    LblResult.Visibility = Visibility.Hidden;   // 결과 레이블 숨김
    if(string.IsNullOrEmpty(TxtUserEmail.Text) || string.IsNullOrEmpty(TxtPassword.Password))  // 아이디나 패스워드 중 하나라도 빠지면 오류처리
    {
        LblResult.Visibility = Visibility.Visible;
        LblResult.Content = "아이디나 패스워드를 입력하세요.";
        Commons.LOGGER.Warn("아이디/패스워드 미입력 접속시도.");
        return;
    }

    try
    {
        var email = TxtUserEmail.Text;
        var password = TxtPassword.Password;

        var mdHash = MD5.Create();  // 패스워드를 암호화해서 DB에 저장(MD5)
        password = Commons.GetMd5Hash(mdHash, password);

        // DB상에 이메일과 패스워드가 같은 데이터가 있는지 확인
        var isOurUser = Logic.DataAccess.GetUser()
            .Where(u => u.UserEmail.Equals(email) && u.UserPassword.Equals(password) 
                        && u.UserActivated == true).Count();
      
        if(isOurUser == 0)
        {
            // 접속 불가 처리
            LblResult.Visibility = Visibility.Visible;
            LblResult.Content = "아이디나 패스워드가 일치하지 않습니다.";
            Commons.LOGGER.Warn("아이디/패스워드 불일치.");
            return;
        }
        else
        {
            // 접속 처리
            Commons.LOGINED_USER = Logic.DataAccess.GetUser().Where(u => u.UserEmail.Equals(email)).FirstOrDefault();
            Commons.LOGGER.Info($"{email} 접속성공");   // 이메일의 접속을 로그로 기록해둠.
            this.Visibility = Visibility.Hidden;
        }
    }

    catch (Exception ex)
    {
        // 예외 처리
        Commons.LOGGER.Error($"예외발생 : {ex}");
        await this.ShowMessageAsync("예외", $"예외 발생 {ex}");
    }
}
```

<br>

--------------

### 계정관리
<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/ERP_Account%EC%8B%A4%ED%96%89%ED%99%94%EB%A9%B4.gif" >
</p>

##### - 계정버튼을 클릭하면 현재 로그인된 계정의 정보가 출력
```C#
private void Page_Loaded(object sender, RoutedEventArgs e)
{
    try
    {
        var user = Commons.LOGINED_USER;
        TxtUserID.Text = user.UserID.ToString();
        TxtUserIdentityNumber.Text = user.UserIdentityNumber.ToString();
        TxtUserSurName.Text = user.UserSurname.ToString();
        TxtUserName.Text = user.UserName.ToString();
        TxtUserEmail.Text = user.UserEmail.ToString();
        TxtUserAdmin.Text = user.UserAdmin.ToString();
        TxtUserActivated.Text = user.UserActivated.ToString();
    }
}
```

<br>

##### - 수정 화면에서 수정 버튼을 눌렀을때 유효성검사와 DB Update

```C#
private void BtnUpdate_Click(object sender, RoutedEventArgs e)
{
    bool isValid = true;

    // 모든 라벨을 숨김
    LblUserIdentityNumber.Visibility = LblUserSurName.Visibility = LblUserName.Visibility =
    LblUserEmail.Visibility = LblUserPassword.Visibility = LblUserAdmin.Visibility =
    LblUserActivated.Visibility = Visibility.Hidden;

    var user = Commons.LOGINED_USER;

    if (string.IsNullOrEmpty(TxtUserSurName.Text))
    {
        LblUserSurName.Visibility = Visibility.Visible;
        LblUserSurName.Text = "이름(성)을 입력하세요.";
        isValid = false;
    }

    if (string.IsNullOrEmpty(TxtUserEmail.Text))
    {
        LblUserEmail.Visibility = Visibility.Visible;
        LblUserEmail.Text = "메일을 입력하세요.";
        isValid = false;
    }

    if (string.IsNullOrEmpty(TxtUserPassword.Password))
    {
        LblUserPassword.Visibility = Visibility.Visible;
        LblUserPassword.Text = "패스워드를 입력하세요.";
        isValid = false;
    }

    if (isValid)
    {
        // 컴포넌트에 값이 잘 입력됬으면 입력된 값으로 DB 데이터 수정
        user.UserSurname = TxtUserSurName.Text;
        user.UserName = TxtUserName.Text;
        user.UserEmail = TxtUserEmail.Text;
        user.UserPassword = TxtUserPassword.Password;
        user.UserAdmin = bool.Parse(CboUserAdmin.SelectedValue.ToString());
        user.UserActivated = bool.Parse(CboUserActivated.SelectedValue.ToString());

        try
        {
            var mdHash = MD5.Create();
            user.UserPassword = Commons.GetMd5Hash(mdHash, user.UserPassword);

            var result = Logic.DataAccess.SetUser(user);
            if(result == 0)
            {
                // 수정 안됨
                LblResult.Text = "계정 수정에 문제가 발생했습니다. 관리자에게 문의 바랍니다.";
                LblResult.Foreground = Brushes.OrangeRed;  // Foreground는 Colors 가 아니라 Brushes임.
            }
            else
            {
                // 정상적으로 수정됨
                LblResult.Text = "정상적으로 계정이 수정되었습니다.";
                LblResult.Foreground = Brushes.DeepSkyBlue;

            }
        }
        catch (Exception ex)
        {
            Commons.LOGGER.Error($"예외 발생 : 사용자 계정 수정 오류 {ex}");
        }

    }
}
```

<br>

--------------

### 사용자관리
<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/ERP_User%EC%8B%A4%ED%96%89%ED%99%94%EB%A9%B4.gif" >
</p>


##### - PDF 익스포트 처리
```C#
using iTextSharp.text;
using iTextSharp.text.pdf;

private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
{
    SaveFileDialog saveDialog = new SaveFileDialog();
    saveDialog.Filter = "PDF FIle(*.pdf)|*.pdf";
    saveDialog.FileName = "";
    if (saveDialog.ShowDialog() == true)
    {
        try
        {
            // 0. PDF에 사용할 폰트 설정
            string nanumPath = Path.Combine(Environment.CurrentDirectory, @"NanumGothic.ttf"); // 폰트 경로 지정
            //                      Combine은 문자열 결함 함수, Environment.CurrentDirectory 현재 작업 폴더에서 프로그램 실행

            BaseFont nanumBase = BaseFont.CreateFont(nanumPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var nanumTitle = new iTextSharp.text.Font(nanumBase, 18f);  // 사이즈 20짜리 타이틀용 나눔폰트 생성
            var nanumContent = new iTextSharp.text.Font(nanumBase, 12f); // 사이즈 12짜리 내용에쓰는 나눔폰트 생성
            
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
                    // UserName
                    // UserEmail
                    // UserAdmin
                    // UserActivated   등 모든 컬럼도 cell에 추가해주면된다.
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
                stream.Close();
            }

            Commons.ShowMessageAsync("PDF변환", "PDF 익스포트 성공했습니다.");
        }
        catch (Exception ex)
        {
            Commons.LOGGER.Error($"예외발생 BtnExportPdf_Click : {ex}");
        }
    }
}
```

<br>

--------------

### 창고관리
<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/ERP_Store%EC%8B%A4%ED%96%89%ED%99%94%EB%A9%B4.gif" width = "83%" height ="83%"  >
</p>

##### - Excel 익스포트 
```C#
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

private void BtnExportExcel_Click(object sender, RoutedEventArgs e)
{
    SaveFileDialog dialog = new SaveFileDialog();
    dialog.Filter = "Excel File (*.xlsx)|*.xlsx";  // 엑셀 확장자
    dialog.FileName = "";
    if (dialog.ShowDialog() == true)
    {
        try
        {
            IWorkbook workbook = new XSSFWorkbook();         // xlsx용      // new HSSFWorkbook(); // xls(이전버전용)    
            ISheet sheet = workbook.CreateSheet("Sheet1");   // sheet1의 이름을 변경가능함

            // 헤더row
            IRow rowHeader = sheet.CreateRow(0);
            ICell cell = rowHeader.CreateCell(0);
            cell.SetCellValue("순번");
            cell = rowHeader.CreateCell(1);
            cell.SetCellValue("창고명");
            cell = rowHeader.CreateCell(2);
            cell.SetCellValue("창고위치");
            cell = rowHeader.CreateCell(3);
            cell.SetCellValue("재고 수");

            for (int i = 0; i<GrdData.Items.Count; i++)  // 실제 데이터 입력
            {
                IRow row = sheet.CreateRow(i + 1);

                if(GrdData.Items[i] is Model.StockStore)
                {
                    var StockStore = GrdData.Items[i] as Model.StockStore;
                    ICell dataCell = row.CreateCell(0);
                    dataCell.SetCellValue(StockStore.StoreID);
                    dataCell = row.CreateCell(1);
                    dataCell.SetCellValue(StockStore.StoreName);
                    dataCell = row.CreateCell(2);
                    dataCell.SetCellValue(StockStore.StoreLocation);
                    dataCell = row.CreateCell(3);
                    dataCell.SetCellValue(StockStore.StockQuantity);
                }
            }

            // 파일저장
            using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }

            Commons.ShowMessageAsync("엑셀저장", "엑셀 export 성공");
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
```


<br><br>

--------------

### DataAccess.cs(User 기준)
- Entity Framework를 활용함으로써 직접 쿼리문을 작성하지않고, ToList(), AddOrUpdate() 메서드로 CRUD 구현
- DB처리하는 클래스를 따로 생성하여 이 클래스만 호출하면 쿼리문을 사용할 수 있도록 구현


```C#
public class DataAccess
{
    // 유저 DB 불러오기
    public static List<User> GetUser()
    {
        List<User> users;
        using(var ctx = new SMSEntities()) 
        {
            users = ctx.User.ToList();  // SELECT * FROM User 랑 똑같은 쿼리문
        }

        return users;
    }


    // 데이터값의 입력, 수정을 동시에하는 메서드
    // 키값이 있으면 UPDATE, 키값이 없으면 INSERT
    public static int SetUser(User user)
    {
        using(var ctx = new SMSEntities())
        {
            ctx.User.AddOrUpdate(user);  // INSERT or UPDATE
            return ctx.SaveChanges();   // 커밋(Commit) 역할하는 구문. (0 또는 1 이상의 값을 돌려줌)
        }
    }
}
```

<br>
    
--------------

### Commons.cs
##### - 로그인한 유저 정보를 컨트롤 박스 옆에 출력

<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/%EC%BB%A8%ED%8A%B8%EB%A1%A4%EB%B0%95%EC%8A%A4%EC%98%86%EC%97%90%EA%B3%84%EC%A0%95%EC%A0%95%EB%B3%B4.png"  
</p>

 <br>
  
  
```C#
public static User LOGINED_USER;  // 객체 User는 DB와 연동할때 Model 폴더에 클래스로 만들어져있음
```

<br>
  
##### - MD5 암호화 메서드 : 패스워드를 암호화해서 DB에 저장합니다.
<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/MD5%20%EC%95%94%ED%98%B8%ED%99%94%20%EC%9D%B4%EB%AF%B8%EC%A7%80(DB).png" 
</p>

<p align = "center">
  (1234, 1111 암호를 위처럼 저장할 수 있습니다)
</p>  
  
```C#
public static string GetMd5Hash(MD5 md5hash, string plainStr) // 암호학에 관련된 구문
{
    byte[] data = md5hash.ComputeHash(Encoding.UTF8.GetBytes(plainStr));
    StringBuilder builder = new StringBuilder();
    for (int i = 0; i < data.Length; i++)
    {
        builder.Append(data[i].ToString("x2"));
    }

    return builder.ToString();
}
```

<br>
                                    
##### - 이메일 정규식 확인 메서드(이메일의 형식으로 잘 입력했는지 확인하는 메서드)                                    
```C#
internal static bool IsValidEmail(string eamil)
{
    return Regex.IsMatch(eamil, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
    //                                                                                             골뱅이 필수    
    // 이메일의 형식을 검사하는 정규식(IsMatch를통해 bool로 리턴함)
}
```

                                    
<br>
  
##### -  Metro MessageBox 공통메서드
```C#
// 메세지다이알로그를 어느 곳에서든 쓸수있게 메서드화(메세지박스 제목, 메세지내용, 출력할 메세지 유형을 파라미터로 받음)
public static async Task<MessageDialogResult> ShowMessageAsync(
    string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
{
    return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
}
```

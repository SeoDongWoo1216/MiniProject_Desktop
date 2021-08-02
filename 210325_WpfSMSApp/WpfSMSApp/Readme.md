## 프로토타입 ERP 프로젝트
- ERP는 전사적 자원 관리(Enterprise Resource Planning)의 약칭으로, 재무, 제조, 소매유통, 공급망, 인사 관리 등의 프로세스를 자동화하고 관리하는 시스템입니다.
- 본 프로젝트는 WPF의 CRUD를 학습하기위해 ERP 일부를 구현한 프로젝트입니다.
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
- [로그인화면](#로그인)에서 ID와 PW를 비교하여 로그인 처리하고, MD5를 통해 DB에 PW가 암호화되고, 이메일 정규식을 확인하는 메서드로 작성했습니다.
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

<br>

--------------

### 사용자관리
<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/ERP_User%EC%8B%A4%ED%96%89%ED%99%94%EB%A9%B4.gif" >
</p>

<br>

--------------

### 창고관리
<p align = "center">
  <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/210325_WpfSMSApp/WpfSMSApp/result_image/ERP_Store%EC%8B%A4%ED%96%89%ED%99%94%EB%A9%B4.gif" width = "83%" height ="83%"  >
</p>

<br><br>

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

```C#
public static string GetMd5Hash(MD5 md5hash, string plainStr) // 이해하기보다는 따라서 쓰기만하기...(암호학에 관련된 구문임)
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

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WpfSMSApp.View
{
    /// <summary>
    /// LoginView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();
            Commons.LOGGER.Info("Login View 초기화!");   // 로그 띄우기
        }

        private async void BtnCancel_Click(object sender, RoutedEventArgs e)  
        {
            // 메트로 마하앱을 쓰기위해 비동기 사용(메세지박스.Show 쓰려면 비동기안해도됨)
            // using MahApps.Metro.Controls.Dialogs; 추가해야함
            var result = await this.ShowMessageAsync("종료", "프로그램을 종료하시겠습니까?",
                                                     MessageDialogStyle.AffirmativeAndNegative, null);

            if (result == MessageDialogResult.Affirmative) // 메세지다이얼로그결과가 1이면
            {
                Commons.LOGGER.Info("프로그램 종로");
                Application.Current.Shutdown();  // 완전 프로그램 종료
            }

            // 메세지박스로 썼을때
            //var result = MessageBox.Show("종료하시겠습니까?", "종료", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            //if(result == MessageBoxResult.OK)
            //    Application.Current.Shutdown();  // 완전 프로그램 종료
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LblResult.Visibility = Visibility.Hidden;   // 결과 레이블 숨김.
            if(string.IsNullOrEmpty(TxtUserEmail.Text) || string.IsNullOrEmpty(TxtPassword.Password))
            {
                LblResult.Visibility = Visibility.Visible;
                LblResult.Content = "아이디나 패스워드를 입력하세요.";
                Commons.LOGGER.Warn("아이디/패스워드 미입력 접속시도.");
                return;
            }

            try
            {
                var email = TxtUserEmail.Text;           // 여기다 브레이크포인트 걸고 디버그 ㄱㄱ
                var password = TxtPassword.Password;
                var isOurUser = Logic.DataAccess.GetUser()
                    .Where(u => u.UserEmail.Equals(email) && u.UserPassword.Equals(password) 
                                && u.UserActivated == true).Count();
                // DB상에 이메일과 패스워드가 같은 데이터가 있는지 확인

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
                    Commons.LOGINED_USER = Logic.DataAccess.GetUser().Where(u => u.UserEmail.Equals(email)).FirstOrDefault();  // 브레이크 포인트후 로그인유저에 마우스갖다대서 확인
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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtUserEmail.Focus();

            LblResult.Visibility = Visibility.Hidden;   // 로그인 결과를 나타내는 라벨을 숨김
        }

        private void TxtUserEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TxtPassword.Focus();        // 이메일치고 엔터누르면 포커스가 패스워드창으로 감
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                BtnLogin_Click(sender, e);  // 패스워드 입력 후 엔터누르면 로그인 버튼 클릭이벤트로 넘어감.
        }
    }
}

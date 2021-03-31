using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;

namespace WpfSMSApp.View.User
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddUser : Page
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 모든 라벨을 숨김
                LblUserIdentityNumber.Visibility = LblUserSurName.Visibility = LblUserName.Visibility =
                LblUserEmail.Visibility = LblUserPassword.Visibility = LblUserAdmin.Visibility =
                LblUserActivated.Visibility = Visibility.Hidden;

                // 콤보박스 초기화
                List<string> comboValues = new List<string>  // 콤보박스에 데이터(트루, 펄스)를 넣을 리스트 선언
                {
                    "false",
                    "True"
                };
                CboUserAdmin.ItemsSource     = comboValues;
                CboUserActivated.ItemsSource = comboValues;

                TxtUserID.Text = TxtUserIdentityNumber.Text = "";
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 EditAccount Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();  // '이전' 버튼을 클릭하면 말그대로 이전화면(계정화면)을 보여줌.
        }

        public bool IsValidInput()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(TxtUserIdentityNumber.Text))
            {
                LblUserIdentityNumber.Visibility = Visibility.Visible;
                LblUserIdentityNumber.Text = "사번을 입력하세요.";
                isValid = false;
            }
            else
            {
                var cnt = Logic.DataAccess.GetUser().Where(u => u.UserIdentityNumber.Equals(TxtUserIdentityNumber.Text)).Count();
                if (cnt > 0)
                {
                    LblUserIdentityNumber.Visibility = Visibility.Visible;
                    LblUserIdentityNumber.Text = "중복된 사번이 존재합니다.";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(TxtUserSurName.Text))
            {
                LblUserSurName.Visibility = Visibility.Visible;
                LblUserSurName.Text = "이름(성)을 입력하세요.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(TxtUserName.Text))
            {
                LblUserName.Visibility = Visibility.Visible;
                LblUserName.Text = "이름을 입력하세요.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(TxtUserEmail.Text))
            {
                LblUserEmail.Visibility = Visibility.Visible;
                LblUserEmail.Text = "메일을 입력하세요.";
                isValid = false;
            }
            else
            {
                var cnt = Logic.DataAccess.GetUser().Where(u => u.UserEmail.Equals(TxtUserEmail.Text)).Count();
                if (cnt > 0)
                {
                    LblUserEmail.Visibility = Visibility.Visible;
                    LblUserEmail.Text = "중복된 이메일이 존재합니다.";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(TxtUserPassword.Password))
            {
                LblUserPassword.Visibility = Visibility.Visible;
                LblUserPassword.Text = "패스워드를 입력하세요.";
                isValid = false;
            }

            if (CboUserAdmin.SelectedIndex < 0)
            {
                LblUserAdmin.Visibility = Visibility.Visible;
                LblUserAdmin.Text = "관리자 여부를 선택하세요.";
                isValid = false;
            }

            if (CboUserActivated.SelectedIndex < 0)
            {
                LblUserActivated.Visibility = Visibility.Visible;
                LblUserActivated.Text = "활성여부를 선택하세요.";
                isValid = false;
            }


            if (!Commons.IsValidEmail(TxtUserEmail.Text))  // 이메일 유효성 검사
            {
                LblUserEmail.Visibility = Visibility.Visible;
                LblUserEmail.Text = "이메일 형식이 올바르지 않습니다.";
                isValid = false; // isValid가 false면 못빠져나감.
            }

            return isValid;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // 모든 라벨을 숨김
            LblUserIdentityNumber.Visibility = LblUserSurName.Visibility = LblUserName.Visibility =
            LblUserEmail.Visibility = LblUserPassword.Visibility = LblUserAdmin.Visibility =
            LblUserActivated.Visibility = Visibility.Hidden;

            var user = new Model.User();  // 새롭게 사용자(데이터)가 INSERT되기위해 객체 생성

            isValid = IsValidInput();     // 컴포넌트들을 제대로 입력했는지 확인


            // 유효성 체크(제대로 잘 들어갔는지 확인하는 구문)
            if (isValid)
            {
                // 컴포넌트에 값이 잘 입력됬으면 입력된 값으로 DB 데이터 수정
                user.UserIdentityNumber = TxtUserIdentityNumber.Text;
                user.UserSurname = TxtUserSurName.Text;
                user.UserName = TxtUserName.Text;
                user.UserEmail = TxtUserEmail.Text;
                user.UserPassword = TxtUserPassword.Password;
                user.UserAdmin = bool.Parse(CboUserAdmin.SelectedValue.ToString());
                user.UserActivated = bool.Parse(CboUserActivated.SelectedValue.ToString());

                try
                {
                    var mdHash = MD5.Create(); // 암호화
                    user.UserPassword = Commons.GetMd5Hash(mdHash, user.UserPassword);

                    var result = Logic.DataAccess.SetUser(user);
                    if (result == 0)
                    {
                        // 사용자 데이터 입력 안됨(비정상적 작동)
                        LblResult.Text = "사용자 입력에 문제가 발생했습니다. 관리자에게 문의 바랍니다.";
                        LblResult.Foreground = Brushes.OrangeRed;  // Foreground는 Colors 가 아니라 Brushes임.
                    }
                    else
                    {
                        // 정상적으로 수정되면 원래화면(앞에 화면)으로 바꿈
                        NavigationService.Navigate(new UserList());
                    }
                }

                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외 발생 : 사용자 계정 수정 오류 {ex}");
                }

            }
        }
    }
}

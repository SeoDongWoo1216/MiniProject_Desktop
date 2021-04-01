using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;

namespace WpfSMSApp.View.Store
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddStore : Page
    {
        public AddStore()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 모든 라벨을 숨김
                LblUserIdentityNumber.Visibility = LblUserSurName.Visibility = Visibility.Hidden;

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

            

            return isValid;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // 모든 라벨을 숨김
            LblUserIdentityNumber.Visibility = LblUserSurName.Visibility = Visibility.Hidden;

            var user = new Model.User();  // 새롭게 사용자(데이터)가 INSERT되기위해 객체 생성

            isValid = IsValidInput();     // 컴포넌트들을 제대로 입력했는지 확인


            // 유효성 체크(제대로 잘 들어갔는지 확인하는 구문)
            if (isValid)
            {
                // 컴포넌트에 값이 잘 입력됬으면 입력된 값으로 DB 데이터 수정
                user.UserIdentityNumber = TxtUserIdentityNumber.Text;
                user.UserSurname = TxtUserSurName.Text;
                

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
                        // NavigationService.Navigate(new UserList());
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

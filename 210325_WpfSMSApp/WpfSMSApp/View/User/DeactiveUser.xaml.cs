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
    public partial class DeactiveUser : Page
    {
        public DeactiveUser()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Model.User> users = Logic.DataAccess.GetUser();
                this.DataContext = users;
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



        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
           
        }


        // 그리드에서 클릭한 값이 입력창에 나오도록하는 이벤트
        private void GrdData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var user = GrdData.SelectedItem as Model.User;  // 그리드의 값들을 user에 다 넣어줌
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 GrdData_SelectionChanged :{ex} ");
            }
           
        }

        private async void BtnDeactive_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (GrdData.SelectedItem == null)
            {
                await Commons.ShowMessageAsync("오류", "비활성화할 사용자를 선택하세요");
                return;
            }



            // 유효성 체크(제대로 잘 들어갔는지 확인하는 구문)
            if (isValid)
            {
                try
                {
                    var user = GrdData.SelectedItem as Model.User;
                    user.UserActivated = false;

                    var result = Logic.DataAccess.SetUser(user);

                    if (result == 0)
                    {
                        // 사용자 데이터 입력 안됨(비정상적 작동)
                        await Commons.ShowMessageAsync("오류", "사용자 수정에 실패했습니다.");

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

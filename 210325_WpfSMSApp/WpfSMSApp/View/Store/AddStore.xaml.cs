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
            // 모든 라벨을 숨김
            LblStoreName.Visibility = LblStoreLocation.Visibility = Visibility.Hidden;
            TxtStoreID.Text = TxtStoreName.Text = TxtStoreLocation.Text ="";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();  // '이전' 버튼을 클릭하면 말그대로 이전화면(계정화면)을 보여줌.
        }


        public bool IsValidInput()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(TxtStoreName.Text))
            {
                LblStoreName.Visibility = Visibility.Visible;
                LblStoreName.Text = "창고명을 입력하세요.";
                isValid = false;
            }
            else
            {
                // 오류있을것같은 구간
                var cnt = Logic.DataAccess.GetStores().Where(u => u.StoreName.Equals(TxtStoreName.Text)).Count();
                if (cnt > 0)
                {
                    LblStoreName.Visibility = Visibility.Visible;
                    LblStoreName.Text = "중복된 창고명이 존재합니다.";
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(TxtStoreLocation.Text))
            {
                LblStoreLocation.Visibility = Visibility.Visible;
                LblStoreLocation.Text = "창고위치를 입력하세요.";
                isValid = false;
            }

            

            return isValid;
        }





        bool IsValid = true;  // 지역 변수를 전역 변수로 설정(i -> I 대문자로 구분)

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

            // 모든 라벨을 숨김
            LblStoreName.Visibility = LblStoreLocation.Visibility = Visibility.Hidden;

            var store = new Model.Store();  // 새롭게 사용자(데이터)가 INSERT되기위해 객체 생성

            IsValid = IsValidInput();     // 컴포넌트들을 제대로 입력했는지 확인(유효성 체크, 개발자에게 제일 중요함!)


            // 유효성 체크(제대로 잘 들어갔는지 확인하는 구문)
            if (IsValid)
            {
                // 컴포넌트에 값이 잘 입력됬으면 입력된 값으로 DB 데이터 수정
                store.StoreName = TxtStoreName.Text;
                store.StoreLocation = TxtStoreLocation.Text;

                try
                {
                    var result = Logic.DataAccess.SetStore(store);     //Logic.DataAccess.SetUser(user);
                    if (result == 0)
                    {
                        // 사용자 데이터 입력 안됨(비정상적 작동)
                        Commons.LOGGER.Error($"AddStore.xaml.s 창고정보 저장오류 발생");
                        Commons.ShowMessageAsync("오류", "저장시 오류가 발생했습니다.");
                        return;
                    }
                    else
                    {
                        // 정상적으로 수정되면 원래화면(앞에 화면)으로 바꿈
                        NavigationService.Navigate(new StoreList());
                        // NavigationService.Navigate(new UserList());
                    }
                }

                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외 발생 : 창고정보 저장 오류 발생 : {ex}");
                }

            }
        }


        // LostFocus : 버튼 누르기 전에 탭을 눌러서 넘어가면 Lbl이 미리 출력된다
        // ('중복된 창고명존재'나 '창고명을 입력하라'는 메세지가 미리 출력)
        private void TxtStoreName_LostFocus(object sender, RoutedEventArgs e)
        {
            IsValidInput();
        }

        private void TxtStoreLocation_LostFocus(object sender, RoutedEventArgs e)
        {
            IsValidInput();
        }
    }
}

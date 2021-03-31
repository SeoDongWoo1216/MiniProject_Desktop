using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;  // 정규식 쓰는 참조
using System.Threading.Tasks;
using System.Windows;
using WpfSMSApp.Model;

namespace WpfSMSApp
{
    class Commons
    {
        // NLog정적 인스턴스 생성
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        
        // 로그인한 유저 정보
        public static User LOGINED_USER;  // 객체 User는 DB와 연동할때 Model 폴더에 클래스로 만들어져있음


        // MD5 암호화 메서드(비번을 암호화해서 DB에 저장)
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


        // 이메일 정규식 확인 메서드(이메일의 형식으로 입력했는지 확인)
        internal static bool IsValidEmail(string eamil)
        {
            return Regex.IsMatch(eamil, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
            //                                                                                             골뱅이 필수    
            // 이메일의 형식을 검사하는 정규식(IsMatch를통해 bool로 리턴함)
        }


        // Metro MessageBox 공통메서드
        // 메세지다이알로그를 어느 곳에서든 쓸수있게 메서드화(메세지박스 제목, 메세지내용, 출력할 메세지 유형을 파라미터로 받음)
        public static async Task<MessageDialogResult> ShowMessageAsync(
            string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }

    }
}

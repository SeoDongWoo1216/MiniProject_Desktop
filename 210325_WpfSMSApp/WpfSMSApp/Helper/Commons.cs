using NLog;
using System.Security.Cryptography;
using System.Text;
using WpfSMSApp.Model;

namespace WpfSMSApp
{
    class Commons
    {
        // NLog정적 인스턴스 생성
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        // 로그인한 유저 정보
        public static User LOGINED_USER;  // 객체 User는 DB와 연동할때 Model 폴더에 클래스로 만들어져있음


        // 비번을 암호화해서 DB에 저장.
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

    }
}

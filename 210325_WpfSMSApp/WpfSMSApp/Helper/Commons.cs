using NLog;
using WpfSMSApp.Model;

namespace WpfSMSApp
{
    class Commons
    {
        // NLog정적 인스턴스 생성
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        // 로그인한 유저 정보
        public static User LOGINED_USER;  // 객체 User는 DB와 연동할때 Model 폴더에 클래스로 만들어져있음
    }
}

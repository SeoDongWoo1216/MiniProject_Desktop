using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSMSApp.Model;  // 리스트를 쓰기위해 참조추가

namespace WpfSMSApp.Logic
{
    public class DataAccess
    {
        public static List<User> GetUser()
        {
            List<User> users;

            using(var ctx = new SMSEntities())  // 여기 브레이크 포인트(users에 마우스갖다대서 데이터 확인)
            {
                users = ctx.User.ToList();  // SELECT * FROM User 랑 똑같은 쿼리문
            }

            return users;
        }
    }

}

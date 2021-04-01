using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSMSApp.Model;  // 리스트를 쓰기위해 참조추가

namespace WpfSMSApp.Logic
{
    public class DataAccess
    {
        // 유저 DB 불러오기
        public static List<User> GetUser()
        {
            List<User> users;

            using(var ctx = new SMSEntities())  // 여기 브레이크 포인트(users에 마우스갖다대서 데이터 확인)
            {
                users = ctx.User.ToList();  // SELECT * FROM User 랑 똑같은 쿼리문
            }

            return users;
        }


        // 데이터값의 입력, 수정을 동시에하는 메서드
        // 키값이 있으면 UPDATE, 키값이 없으면 INSERT
        public static int SetUser(User user)
        {
            using(var ctx = new SMSEntities())
            {
                ctx.User.AddOrUpdate(user);
                return ctx.SaveChanges();   // 커밋(Commit) 역할하는 구문. (0 또는 1 이상의 값을 돌려줌)
            }
        }

      


        // 창고 DB 불러오기
        public static List<Store> GetStores()
        {
            List<Store> stores;

            using(var ctx = new SMSEntities())
            {
                stores = ctx.Store.ToList();
            }

            return stores;
        }

        public static int SetStore(Store store)
        {
            using(var ctx = new SMSEntities())
            {
                ctx.Store.AddOrUpdate(store);
                return ctx.SaveChanges();
            }
        }

        public static List<Stock> GetStocks()
        {
            List<Stock> stocks;

            using (var ctx = new SMSEntities())
            {
                stocks = ctx.Stock.ToList();
            }

            return stocks;
        }



        // DB 불러오기
    }
}

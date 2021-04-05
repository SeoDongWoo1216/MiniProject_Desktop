using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace _210401_NaverMovieFinderApp
{
    public class Commons
    {
       
        public static bool IsFavorite = false;  // 즐겨찾기 여부 플래그

        public static bool IsDelete = false;    // 즐겨찾기 삭제와 보기 플래그

        // NLog 정적 객체
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        public static async Task<MessageDialogResult> ShowMessageAsync(
            string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }


        public static string GetOpenApiResult(string openApiUrl, string clientID, string clientSeret)
        {
            var result = "";

            try
            {
                WebRequest request = WebRequest.Create(openApiUrl);
                request.Headers.Add("X-Naver-Client-Id", clientID); // 무조건 네이버에서 받을려면 넣어줘야함.
                request.Headers.Add("X-Naver-Client-Secret", clientSeret);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                result = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생 : {ex.Message}");
            }

            return result;
        }


        // 불러온 데이터에서 HTML 태그를 삭제해주는 메서드
        public static string StripHtmlTag(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", "");  // HTML 태그 삭제하는 정규 표현식
        }

        public static string StripPipe(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";   // 빈값이 있을때는 그대로 출력
            else
            {
                return text.Substring(0, text.LastIndexOf("|")).Replace("|", ", ");  // 마지막 '|' 를 뺀다음에 '|'를 ', ' 로 바꿔줌  
            }



            //if (string.IsNullOrEmpty(text))
            //    return "";
            //return text.Replace("&amp;", "");

            /*string result = text.Replace("|",", ");
            return result.Substring(0, result.LastIndexOf(","));*/
        }
    }
}

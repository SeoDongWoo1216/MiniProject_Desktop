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

namespace _210401_NaverMovieFinderApp
{
    public class Commons
    {

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
    }
}

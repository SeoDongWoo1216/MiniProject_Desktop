using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleNaverMovieFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            string ClientID = "U5xL8wYH5Evo2TMHUB23";
            string clientSeret = "gy9PKhYriJ";
            string search = "starwars"; // 변경 가능
            string openApiUrl = $"https://openapi.naver.com/v1/search/movie?query={search}";

            var responseJson = GetOpenApiResult(openApiUrl, ClientID, clientSeret);

            // string을 읽어서 Json으로 변환
            JObject parsedJson = JObject.Parse(responseJson);

            int total = Convert.ToInt32(parsedJson["total"]);
            Console.WriteLine($"총 검색 결과는 {total} 입니다.");
            
            int display = Convert.ToInt32(parsedJson["display"]);
            Console.WriteLine($"화면 출력 : {display}");

            var items = parsedJson["items"];
            JArray json_array = (JArray)items;
            foreach (var item in json_array)
            {
                Console.WriteLine($"{item["title"]} / {item["image"]} / {item["subtitle"]} / {item["actor"]}");
            }
        }

        private static string GetOpenApiResult(string openApiUrl, string clientID, string clientSeret)
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

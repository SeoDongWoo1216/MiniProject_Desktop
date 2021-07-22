## Naver Movie Finder

- Naver API를 사용하여 영화 정보 조회, 네이버 영화에서 검색 가능하도록 구현

    <img src="NaverMovieFinderApp/ref_image/mainwindow.png" width="80%" height="80%"></img>

    [NaverMovieAPI](NaverMovieFinderApp/NaverMovieFinderApp/MainWindow.xaml.cs)

```C#
/* Naver API  */

// 사용자 정보 입력
string clientID = "API 아이디";
string clientSecret = "API 비밀번호";

string openApiUrl = $"https://openapi.naver.com/v1/search/movie?start=1&display=30&query={movieName}";

// request 요청
string resJson = Commons.GetOpenApiResult(openApiUrl, clientID, clientSecret);
var parsedJson = JObject.Parse(resJson);

int total = Convert.ToInt32(parsedJson["total"]);
int display = Convert.ToInt32(parsedJson["display"]);

StsResult.Content = $"{total} 중 {display} 호출 성공";

JToken items = parsedJson["items"];
JArray json_array = (JArray)items;

List<MovieItem> movieItems = new List<MovieItem>();

foreach (var item in json_array)
{
    MovieItem movie = new MovieItem(
        Commons.StripHTMLTag(item["title"].ToString()),
        item["link"].ToString(),
        item["image"].ToString(),
        item["subtitle"].ToString(),
        item["pubDate"].ToString(),
        Commons.StripPipe(item["director"].ToString()),
        Commons.StripPipe(item["actor"].ToString()),
        item["userRating"].ToString()
        );
    movieItems.Add(movie);
}

this.DataContext = movieItems;
```

- DB를 이용해서 즐겨찾기 추가, 삭제, 보기 기능 구현

    <img src="NaverMovieFinderApp/ref_image/watchlist.png" width="80%" height="80%"></img>

    [Entity Framework](NaverMovieFinderApp/NaverMovieFinderApp/MainWindow.xaml.cs)

    <img src="NaverMovieFinderApp/ref_image/DB_SELECT.png" width="80%" height="80%"></img>

- Youtube API를 사용하여 해당 영화의 티저 재생

    <img src="NaverMovieFinderApp/ref_image/youtube.png" width="80%" height="80%"></img>

    [YoutubeTrailerWindow](NaverMovieFinderApp/NaverMovieFinderApp/TrailerWindow.xaml.cs)

```C#
/* Youtube API */

var youtubeService = new YouTubeService(
    new BaseClientService.Initializer()
    {
        ApiKey = "개인키",
        ApplicationName = this.GetType().ToString()
    });

var request = youtubeService.Search.List("snippet");    // snippet 조각
request.Q = LblMovieName.Content.ToString();   // {영화이름} 예고편
request.MaxResults = 10; // 사이즈가 크면 멈춰버림

var response = await request.ExecuteAsync(); // 검색시작(youtube OpenAPI 호출)

foreach (var item in response.Items)
{
    if (item.Id.Kind.Equals("youtube#video"))
    {
        YoutubeItem youtube = new YoutubeItem()
        {
            Title = item.Snippet.Title,
            Author = item.Snippet.ChannelTitle,
            URL = $"https://www.youtube.com/watch?v={item.Id.VideoId}"
        };
        // thumbnail image
        youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute));
    
        youtubes.Add(youtube);
    }
}
```

--------------

## Commons.cs

* <h3> OpenAPI Request & Response 메소드 </h3>

```C#
public static string GetOpenApiResult(string openApiUrl, string clientID, string clientSecret)
{
    var result = "";

    try
    {
        WebRequest request = WebRequest.Create(openApiUrl);
        request.Headers.Add("X-Naver-Client-Id", clientID);
        request.Headers.Add("X-Naver-Client-Secret", clientSecret);

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
        Console.WriteLine($"예외발생: {ex}");
    }
    return result;
}
```

* <h3> 비밀번호 암호화 메소드 </h3>

```C#
public static string GetMd5Hash(MD5 md5Hash, string plainStr)
{
    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plainStr));
    StringBuilder builder = new StringBuilder();
    for (int i = 0; i < data.Length; i++)
    {
        builder.Append(data[i].ToString("x2")); // 16진수로 바꿈
    }

    return builder.ToString();
}
```

* <h3> 이메일 정규식 체크 메소드 </h3>

```C#
public static bool IsValidEmail(string email)
{
    return Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
}
```

* <h3> HTML 태그 삭제 정규식 메소드 </h3>

```C#
public static string StripHTMLTag(string text)
{
    text = Regex.Replace(text, @"<(.|\n)*?>", "");  // HTML 태그
    text = Regex.Replace(text, "&amp;", "&");       // &amp;
    return text;
}
```

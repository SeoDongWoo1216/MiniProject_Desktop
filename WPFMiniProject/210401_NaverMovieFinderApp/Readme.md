# Naver Movie Finder
- Naver Movie API를 사용하여 영화 데이터를 즐겨찾기 기능으로 CRUD 구현
- YouTube API를 사용하여 해당되는 영화의 예고편을 YouTube로 감상할 수 있게 새로운 창으로 띄워서 구현
- '네이버 영화' 버튼으로 네이버 영화 사이트로 링크를 연결하여 해당되는 영화의 평점이나 후기를 볼 수 있도록 구현

---------

## 실행화면
![즐겨찾기](https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/210401_NaverMovieFinderApp_result_001.gif "즐겨찾기 이벤트(인셉션검색으로 시작)")
![예고편보기](https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/210401_NaverMovieFinderApp_result_002.gif "예고편(예고편보기 버튼으로 시작)")

---------

# 소스 코드 분석

## Naver API 접근
- 네이버 개발자 센터에서 아이디와 비밀번호를 발급받고 영화의 데이터를 불러올 수 있다.
```C#
/* Naver API  */

// 사용자 정보 입력
 private void ProcSearchNaverAPI(string movieName)
        {
            // API 불러오기
            string ClientID = "발급받은 API 아이디";
            string clientSecret = "발급받은 API 비밀번호";
            string openApiUrl = $"https://openapi.naver.com/v1/search/movie?start=1&display=30&query={movieName}";
            string resJson = Commons.GetOpenApiResult(openApiUrl, ClientID, clientSecret);

            var parsedJson = JObject.Parse(resJson);

            int total = Convert.ToInt32(parsedJson["total"]);
            int display = Convert.ToInt32(parsedJson["display"]);

            StsResult.Content = $"{total} 중 {display} 호출 성공";

            var items = parsedJson["items"];
            var Json_array = (JArray)items;

            List<MovieItem> movieItems = new List<MovieItem>();  // API에서 불러온 데이터들을 저장할 리스트선언
            foreach (var item in Json_array)
            {
                MovieItem movie = new MovieItem(
                    Commons.StripHtmlTag(item["title"].ToString()),
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
            this.DataContext = movieItems;  // 그리드에 데이터 바인딩(GrdData 대신 this로 해도됨)
        }
```
<p align = "center" >
   <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/Console_NaverMovieAPI.png" 
        width="70%" height="70%"></img>
</p>
<p align = "center" >
소스코드에는 그리드에 데이터를 출력하는데, 콘솔로 받아온 데이터를 출력하면 위 화면처럼 출력된다
</p>                                       

<br><br>

---------------------------------------

### 영화 데이터를 그리드에 출력(BtnSearch_Click)
<p align = "center" >
    <img src="https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/Movie_Search.png" width="80%" height="80%"></img>
</p>
<p align = "center" >
'인셉션'을 검색창에 입력하고, 검색 버튼을 클릭하거나 엔터를 눌렀을시 그리드에 출력된다 <br><br>
</p>

영화를 검색창에 입력했을때 ProcSearchNaverAPI('인셉션'); 메서드를 통해 Naver API에 접근하게된다. 

<br> <br>
-------------------------------------
    
### DB와 연동하여 즐겨찾기 기능으로 CRUD 구현
- Entity Framework를 활용하여 쿼리를 직접 입력할 필요없이 Select, Update, Insert, Delete가 가능하다.
- BtnAddWatchList_Click, BtnViewWatchList_Click, BtnDelete_Click 버튼 클릭 이벤트로 구현
<p align = "center" >
    <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/Movie_Insert_Select.png">
</p>
<p align = "center" >
    즐겨찾기 추가, 보기, 삭제 버튼으로 CRUD 구현(화면은 즐겨찾기 보기 이벤트)
</p>

```C#
// Insert
List<NaverFavoriteMovies> list = new List<NaverFavoriteMovies>();  //  DB의 값을 저장할 리스트 선언
foreach (MovieItem item in GrdData.SelectedItems)
{
      NaverFavoriteMovies temp = new NaverFavoriteMovies()
      {
            Title = item.Title,
            Link = item.Link,
            Image = item.Image,
            SubTitle = item.SubTitle,
            PubDate = item.PubDate,
            Director = item.Director,
            Actor = item.Actor,
            UserRating = item.UserRating,
            RegDate = DateTime.Now
      };

      list.Add(temp);
}
using (var ctx = new OpenApiLabEntities())
{
    // ctx.NaverFavoriteMovies.AddRange(list); // 아래와 차이없음.(Add와 AddRange는 1개와 여러개 추가라는 차이가있음)
    ctx.Set<NaverFavoriteMovies>().AddRange(list); 
    ctx.SaveChanges();
}
```

<br>

```C#
// Select
List<NaverFavoriteMovies> list = new List<NaverFavoriteMovies>(); //  DB의 값을 저장할 리스트 선언
try
{
    using (var ctx = new OpenApiLabEntities())
    {
         list = ctx.NaverFavoriteMovies.ToList();   // DB 값을 출력
    }
    this.DataContext = list;   // 그리드에 데이터 출력
}
```

<br>

```C#
// Delete
foreach (NaverFavoriteMovies item in GrdData.SelectedItems)
{
     using (var ctx = new OpenApiLabEntities())
     {
         // ctx.NaverFavoriteMovies.Remove(item);
         var itemDelete = ctx.NaverFavoriteMovies.Find(item.Idx); // 삭제할 영화 객체 검색 후 생성
         ctx.Entry(itemDelete).State = EntityState.Deleted;      // 검색 객체상태를 삭제로 변경
         ctx.SaveChanges(); // commit
      }
}
```

<br><br>


----------------
### Youtube API를 사용하여 해당 영화의 티저 재생
- YouTubeItem class를 선언하여 객체 생성
- BtnWatchTrailer_Click 버튼 클릭 이벤트로 구현

<p align = "center" >
    <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/Movie_Trailer.png">
</p>
<p align = "center" >
    그리드에 원하는 영화를 선택후에 '예고편 보기' 버튼을 클릭하면 새로운 창이 뜨면서 트레일러가 출력된다
</p>

```C#
/* Youtube API */

private async Task LoadDataCollection()
{
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                 ApiKey = "AIzaSyB9qpfmZ9Ok_QTdnFIIteOW2_nyJnwBcDI",
                 ApplicationName = this.GetType().ToString()
            });

            var request = youtubeService.Search.List("snippet"); 
            request.Q = LblMovieName.Content.ToString();   // 쿼리의 Q, {영화이름} 예고편
            request.MaxResults = 10;   // 사이즈가 크면 멈춰버림.

            var response = await request.ExecuteAsync(); // 검색 시작(youtube OpenAPI 호출)

            foreach (var item in response.Items)
            {
                if (item.Id.Kind.Equals("youtube#video"))
                {
                    YoutubeItem youtube = new YoutubeItem()
                    {
                        Title = item.Snippet.Title,
                        Author = item.Snippet.ChannelTitle,
                        URL = $"http://www.youtube.com/watch?v={item.Id.VideoId}"
                    };

                    // 썸네일 이미지
                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute));
                    youtubes.Add(youtube);
                }
            }
}
```

<br><br>

--------------

## Commons.cs
* <h3> OpenAPI Request & Response 메소드 </h3>
- Http 요청을위해 HttpWebRequest 객체를 만들고 HttpWebResponses로 받은 뒤, Stream을 열어 StreamReader로 값을 읽어줌
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

<br><br>

--------------

## HTML 데이터 Parsing
- 영화 데이터를 받을때 Json 형식으로 받아오는데, 데이터가 러프한 상태로 받아와서 그리드에 출력하면 해당 이미지처럼 출력된다.
<p align = "center">
 <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/Movie_HtmlParsing01.png" >
</p>

<p align = "center">
 따라서 불필요한 데이터를 걸러주는 파싱작업이 필요하다. 이때 아래 메서드들을 추가하면된다
</p>

<br>

```C#
// 불러온 데이터에서 HTML 태그를 삭제해주는 메서드
public static string StripHtmlTag(string text)
{
     text = Regex.Replace(text, @"<(.|\n)*?>", "");  // HTML 태그 삭제하는 정규 표현식
     text = Regex.Replace(text, "&amp;", "&");       // &amp;
     return text;
}

public static string StripPipe(string text)
{
     if (string.IsNullOrEmpty(text)) return "";   // 빈값이 있을때는 그대로 출력
     else
     {
          return text.Substring(0, text.LastIndexOf("|")).Replace("|", ", ");  // 마지막 '|' 를 뺀다음에 '|'를 ', ' 로 바꿔줌  
     }
}
```
<p align = "center">
 <img src = "https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/Movie_HtmlParsing02.png" >
</p>

<p align = "center">
깔끔하게 데이터가 파싱되어서 출력된 실행화면
</p>

-----------------

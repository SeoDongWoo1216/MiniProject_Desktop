namespace _210401_NaverMovieFinderApp.Model
{
    public class MovieItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string SubTitle { get; set; }
        public string PubDate { get; set; }
        public string Director { get; set; }  // 감독, 배우가 |로 구별되어있어서 배열로 저장해야함.
        public string Actor { get; set; }
        public string UserRating { get; set; }  // 리뷰점수

        public MovieItem(string title, string link, string image, string subTitle, 
                        string pubDate, string director, string actor, string userRating)
        {
            Title = title;
            Link = link;
            Image = image;
            SubTitle = subTitle;
            PubDate = pubDate;
            Director = director;
            Actor = actor;
            UserRating = userRating;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace _210401_NaverMovieFinderApp.Model
{
    class YoutubeItem
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string URL { get; set; }
        public BitmapImage Thumbnail { get; set; }
    }
}

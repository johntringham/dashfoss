using System;
using System.Collections.Generic;
using System.Text;

namespace DashFoss.Models
{
    public class PostBit
    {
    }

    public class ImageBit : PostBit 
    {
        public string Url { get; set; }
        public string Desc { get; set; }
    }

    public class HtmlTextBit : PostBit
    {
        public string html { get; set; }
    }

    public class BlogNameBit : PostBit
    {
        public string BlogName { get; set; }
    }

    public class VideoBit : PostBit
    {
        public string Url { get; set; }
    }
}

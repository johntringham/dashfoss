using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

        public PhotoInfo Photo { get; set; }

        public ImageBit(string url, string desc, PhotoInfo photo)
        {
            Url = url;
            Desc = desc;
            Photo = photo;
        }
    }

    public class HtmlTextBit : PostBit
    {
        public string html { get; set; }
    }

    public class BlogNameBit : PostBit
    {
        public string BlogName { get; set; }
        public string ProfilePictureUrl { get; set; }

        public BlogNameBit(string blogName, string imageUrl)
        {
            BlogName = blogName;
            ProfilePictureUrl = imageUrl;
        }
    }

    public class VideoBit : PostBit
    {
        public VideoBit(string url)
        {
            this.Url = url;
        }
        public string Url { get; set; }
    }

    public class QuestionAnswerBit : PostBit
    {
        public List<PostBit> QuestionBits { get; set; } = new List<PostBit>();
        public List<PostBit> AnswerBits { get; set; } = new List<PostBit>();
        public string QuestionAsker { get; set; }
    }

    public class AudioBit : PostBit
    {

    }

    public class NotImplementBit : HtmlTextBit
    {
        public NotImplementBit(BasePost basePost)
        {
            this.html = $"UNIMPLEMENTED TYPE: {basePost.GetType().FullName}. thanks :)";
        }
    }


}

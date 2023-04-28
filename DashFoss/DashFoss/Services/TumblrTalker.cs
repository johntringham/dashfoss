using DashFoss.Models;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DashFoss.Services
{
    public class TumblrTalker
    {
        private TumblrClient client;

        public TumblrTalker()
        {
            Console.WriteLine("creating a client, wow");

            this.client = new TumblrClientFactory().Create<TumblrClient>(Keys.CONSUMER_KEY, Keys.CONSUMER_SECRET, new Token(Keys.OAUTH_TOKEN, Keys.OAUTH_SECRET));
            Console.WriteLine("created a client, wow");
        }

        //public async Task<string> GetSomething()
        //{
        //    var userInfo await client.GetUserInfoAsync();
        //    return userInfo.UserName;
        //}

        public async Task<IEnumerable<TumblrPost>> GetMostRecentPosts()
        {
            BasePost[] posts;
            posts = await client.GetDashboardPostsAsync();
            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        public async Task<IEnumerable<TumblrPost>> GetOlderPosts(long sinceId)
        {
            BasePost[] posts;
            posts = await client.GetDashboardPostsAsync(sinceId, DashboardOption.Before);
            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        private TumblrPost ParsePost(BasePost post)
        {
            var bits = new List<PostBit>();
            bits.Add(new HtmlTextBit() { html = $"{post.GetType().Name} - {post.BlogName} " });

            switch (post)
            {
                case PhotoPost p:
                    foreach(var photo in p.PhotoSet)
                    {
                        bits.Add(new ImageBit(photo.OriginalSize.ImageUrl, "desc", photo.OriginalSize));
                    }
                    bits.Add(new HtmlTextBit() { html = p.Caption });

                    break;

                case TextPost p:
                    bits.AddRange(ParseTextPost(p).ToList());
                    //bits.Add(new HtmlTextBit() { html = p.Body });
                    break;

                case VideoPost p:
                    bits.Add(new VideoBit(p.VideoUrl));
                    break;

                case AnswerPost _:
                case AudioPost _:
                case ChatPost _:
                case LinkPost _:
                case QuotePost _:
                    bits.Add(new NotImplementBit(post));
                    break;
            }

            return new TumblrPost() { Author = post.BlogName, Bits = bits, Id = post.Id };
        }

        private IEnumerable<PostBit> ParseTextPost(TextPost p)
        {
            var body = p.Body;
            var title = p.Title;

            var bits = new List<PostBit>();

            if(!p.Trails.Any())
            {
                bits.Add(new HtmlTextBit() { html = $"<h1>{title}</h1>" });
                ParseTubmlrHtml(bits, body);
            }

            foreach (var trail in p.Trails)
            {
                bits.Add(new BlogNameBit(trail.Blog.Name, trail.Blog.Theme.HeaderImage));

                var content = trail.ContentRaw;
                ParseTubmlrHtml(bits, content);

                //foreach (var node in nodesToLookAt)
                //{
                //    if (node.Name == "img")
                //    {
                //        var width = node.GetAttributeValue("data-orig-width", 100);
                //        var height = node.GetAttributeValue("data-orig-height", 100);
                //        bits.Add(new ImageBit(node.GetAttributeValue("src", ""), "text post image", new PhotoInfo() { Height = height, Width = width }));
                //    }
                //    else if (node.Name == "#text")
                //    {
                //        var text = node.GetDirectInnerText();
                //        if (text != null && text != "")
                //        {
                //            bits.Add(new HtmlTextBit() { html = text });
                //        }
                //    }
                //    else if(node.Name == "a")
                //    {
                //        var text = node.GetDirectInnerText();
                //        if (text != null && text != "")
                //        {
                //            bits.Add(new HtmlTextBit() { html = "LINK:" + text });
                //        }
                //    }
                //    else if(node.Name == "figure")
                //    {
                //        var npfData = node.GetAttributeValue("data-npf", "");
                //        if(npfData != null && npfData != "")
                //        {
                //            npfData = npfData.Replace("&quot;", "\"");
                //            var json = JObject.Parse(npfData);
                //            string figureType = json.SelectToken("type").Value<string>();
                //            string url = json.SelectToken("url").Value<string>();

                //            if (figureType == "video")
                //            {
                //                bits.Add(new VideoBit(url));
                //            }
                //        }
                //    }
                //}
            }


            return bits;
        }

        private static string ParseTubmlrHtml(List<PostBit> bits, string content)
        {
            content = content.Replace("<p>", "");
            content = content.Replace("</p>", "<br />");

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);

            var documentNode = doc.DocumentNode;
            var nodesToInspect = new Stack<HtmlNode>();
            nodesToInspect.Push(documentNode);

            while (nodesToInspect.Count > 0)
            {
                var node = nodesToInspect.Pop();
                if (node.Name == "img")
                {
                    var width = node.GetAttributeValue("data-orig-width", 100);
                    var height = node.GetAttributeValue("data-orig-height", 100);
                    bits.Add(new ImageBit(node.GetAttributeValue("src", ""), "text post image", new PhotoInfo() { Height = height, Width = width }));
                }
                else if (node.Name == "#text")
                {
                    var text = node.GetDirectInnerText();
                    if (text != null && text != "")
                    {
                        bits.Add(new HtmlTextBit() { html = text });
                    }
                }
                else if (node.Name == "a")
                {
                    var text = node.GetDirectInnerText();
                    if (text != null && text != "")
                    {
                        bits.Add(new HtmlTextBit() { html = "LINK:" + text });
                    }
                }
                else if (node.Name == "figure")
                {
                    var npfData = node.GetAttributeValue("data-npf", "");
                    if (npfData != null && npfData != "")
                    {
                        npfData = npfData.Replace("&quot;", "\"");
                        var json = JObject.Parse(npfData);
                        string figureType = json.SelectToken("type").Value<string>();
                        string url = json.SelectToken("url").Value<string>();

                        if (figureType == "video")
                        {
                            bits.Add(new VideoBit(url));
                        }
                    }
                    else
                    {
                        foreach (var child in node.ChildNodes.Reverse())
                        {
                            nodesToInspect.Push(child);
                        }
                    }
                }
                else
                {
                    foreach (var child in node.ChildNodes.Reverse())
                    {
                        nodesToInspect.Push(child);
                    }
                }
            }

            return content;
        }
    }
}

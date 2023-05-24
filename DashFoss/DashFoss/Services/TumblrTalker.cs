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
using System.Security.Cryptography.X509Certificates;
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

        public async Task<IEnumerable<TumblrPost>> GetMostRecentPosts()
        {
            BasePost[] posts;
            posts = await client.GetDashboardPostsAsync(includeReblogInfo:true);
            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        public async Task<IEnumerable<TumblrPost>> GetOlderPosts(string sinceId)
        {
            BasePost[] posts;
            posts = await client.GetDashboardPostsAsync(sinceId, DashboardOption.Before, includeReblogInfo: true);
            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        public async Task<IEnumerable<TumblrPost>> GetMostRecentPostsForAuthor(string blogName)
        {
            var posts = (await client.GetPostsAsync(blogName, includeReblogInfo: true)).Result; // note: not an async hack - just some dumb classes

            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        public async Task<IEnumerable<TumblrPost>> GetOlderPostsForAuthor(string blogName, int ignoreFirst)
        {
            BasePost[] posts;
            posts = (await client.GetPostsAsync(blogName, ignoreFirst, includeReblogInfo: true)).Result;
            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        public async Task<IEnumerable<TumblrPost>> GetLikes()
        {
            var posts = (await client.GetLikesAsync()).Result; // note: not an async hack - just some dumb classes
            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        public async Task<IEnumerable<TumblrPost>> GetOlderLikes(int ignoreFirst)
        {
            BasePost[] posts;
            posts = (await client.GetLikesAsync(ignoreFirst)).Result;
            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        public async Task DoLike(TumblrPost post)
        {
            if (long.TryParse(post.Id, out long id))
            {
                await client.LikeAsync(id, post.BasePost.ReblogKey);
                post.Liked = true;
            }
            else
            {
                throw new InvalidOperationException("post has funky id number, dunno why. number:" + post.Id);
            }
        }

        public async Task DoUnlike(TumblrPost post)
        {
            if (long.TryParse(post.Id, out long id))
            {
                await client.UnlikeAsync(id, post.BasePost.ReblogKey);
                post.Liked = false;
            }
            else
            {
                throw new InvalidOperationException("post has funky id number, dunno why. number:" + post.Id);
            }
        }

        private TumblrPost ParsePost(BasePost post)
        {
            var bits = new List<PostBit>();
            //bits.Add(new HtmlTextBit() { html = $"{post.GetType().Name} - {post.BlogName} " });

            switch (post)
            {
                case PhotoPost p:
                    foreach(var photo in p.PhotoSet)
                    {
                        bits.Add(new ImageBit(photo.OriginalSize.ImageUrl, "desc", photo.OriginalSize));
                    }

                    // todo: this messes up links to author blogs
                    //ParseTumblrHtml(bits, p.Caption);
                    ParseTrails(bits, p.Trails);

                    //bits.Add(new HtmlTextBit() { html = p.Caption });

                    break;

                case TextPost p:
                    bits.AddRange(ParseTextPost(p).ToList());
                    //bits.Add(new HtmlTextBit() { html = p.Body });
                    break;

                case VideoPost p:
                    bits.Add(new VideoBit(p.VideoUrl));
                    ParseTrails(bits, p.Trails);
                    break;

                case AnswerPost p:
                    var qaBit = new QuestionAnswerBit();
                    qaBit.QuestionAsker = p.AskingName;

                    ParseTumblrHtml(qaBit.QuestionBits, p.Question);
                    ParseTumblrHtml(qaBit.AnswerBits, p.Answer);

                    bits.Add(qaBit);
                    ParseTrails(bits, p.Trails);
                    break;

                case AudioPost p:
                    bits.Add(new NotImplementBit(post));
                    ParseTrails(bits, post.Trails);
                    break;

                case ChatPost _:
                case LinkPost _:
                case QuotePost _:
                    bits.Add(new NotImplementBit(post));
                    ParseTrails(bits, post.Trails);
                    break;
            }

            return new TumblrPost() { Author = post.BlogName, Bits = bits, Id = post.Id, Notes = post.NotesCount, RebloggedFrom = post.RebloggedFromName, Tags = post.Tags.ToList(), BasePost = post };
        }

        private IEnumerable<PostBit> ParseTextPost(TextPost p)
        {
            var body = p.Body;
            var title = p.Title;

            var bits = new List<PostBit>();

            List<Trail> trails = p.Trails;

            if (!trails.Any())
            {
                bits.Add(new HtmlTextBit() { html = $"<h1>{title}</h1>" });
                ParseTumblrHtml(bits, body);
            }

            ParseTrails(bits, trails);

            return bits;
        }

        private static void ParseTrails(List<PostBit> bits, List<Trail> trails)
        {
            if (trails != null)
            {
                foreach (var trail in trails)
                {
                    bits.Add(new BlogNameBit(trail.Blog.Name));

                    var content = trail.ContentRaw;
                    ParseTumblrHtml(bits, content);
                }
            }
        }

        private static string ParseTumblrHtml(List<PostBit> bits, string content)
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
                        bits.Add(new HtmlTextBit() { html = "LINK:" + node.OuterHtml });
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

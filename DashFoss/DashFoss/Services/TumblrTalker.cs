using DashFoss.Models;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using FFImageLoading.Helpers.Gif;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Xamarin.Forms;

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

            try
            {
                posts = await client.GetDashboardPostsAsync(includeReblogInfo: true);
            }
            catch (Exception ex)
            {
                await DisplayErrorMessage();
                return new List<TumblrPost>();
            }

            var parsed = posts.Select(p => ParsePost(p));
            return parsed;
        }

        public async Task<bool> DoesFollow(string blog)
        {
            try
            {
                //var userInfo = await client.GetUserInfoAsync();

                //var pfollowsz = await client.GetAreYouFollowing("pukicho");
                //var followPizza = await client.GetAreYouFollowing("kitzenvoncatzen");
                //var zfollowsp = await client.GetFollowedByAsync("pukicho", "zappablamma");

                return false;

                //var following = await client.GetFollowingAsync(0, );
                //return following.Result.Any(f => f.Name == blog);
            }
            catch (Exception e)
            {
                await DisplayErrorMessage();
                return false;
            }
        }

        private static async Task DisplayErrorMessage()
        {
            await Shell.Current.CurrentPage.DisplayAlert("aw man", "connection failed... :(", "ok");
        }

        public async Task<IEnumerable<TumblrPost>> GetOlderPosts(string sinceId)
        {
            try {
                BasePost[] posts;
                posts = await client.GetDashboardPostsAsync(sinceId, DashboardOption.Before, includeReblogInfo: true);
                var parsed = posts.Select(p => ParsePost(p));
                return parsed;
            }
            catch (Exception ex)
            {
                await DisplayErrorMessage();
                return new List<TumblrPost>();
            }
        }

        public async Task<IEnumerable<TumblrPost>> GetMostRecentPostsForAuthor(string blogName)
        {
            try
            {
                var posts = (await client.GetPostsAsync(blogName, includeReblogInfo: true)).Result; // note: not an async hack - just some dumb classes

                var parsed = posts.Select(p => ParsePost(p));
                return parsed;
            }
            catch (Exception ex)
            {
                await DisplayErrorMessage();
                return new List<TumblrPost>();
            }
        }

        public async Task<IEnumerable<TumblrPost>> GetOlderPostsForAuthor(string blogName, int ignoreFirst)
        {
            try
            {
                BasePost[] posts;
                posts = (await client.GetPostsAsync(blogName, ignoreFirst, includeReblogInfo: true)).Result;
                var parsed = posts.Select(p => ParsePost(p));
                return parsed;
            }
            catch (Exception ex)
            {
                await DisplayErrorMessage();
                return new List<TumblrPost>();
            }
        }

        public async Task<IEnumerable<TumblrPost>> GetLikes()
        {
            try
            {
                var posts = (await client.GetLikesAsync()).Result; // note: not an async hack - just some dumb classes
                var parsed = posts.Select(p => ParsePost(p));
                return parsed;
            }
            catch (Exception ex)
            {
                await DisplayErrorMessage();
                return new List<TumblrPost>();
            }
        }

        public async Task<IEnumerable<TumblrPost>> GetOlderLikes(int ignoreFirst)
        {
            BasePost[] posts;

            try
            {
                posts = (await client.GetLikesAsync(ignoreFirst)).Result;
                var parsed = posts.Select(p => ParsePost(p));
                return parsed;
            }
            catch (Exception ex)
            {
                await DisplayErrorMessage();
                return new List<TumblrPost>();
            }
        }

        public async Task<TumblrPost> LoadNotes(TumblrPost post)
        {
            try
            {
                var newPost = await client.GetPostAsync(post.BasePost.BlogName, long.Parse(post.Id), true, true);
                post.Notes = newPost.Notes;
                return post;
            }
            catch (Exception ex)
            {
                await DisplayErrorMessage();
                return post;
            }
        }

        public async Task DoLike(TumblrPost post)
        {
            if (long.TryParse(post.Id, out long id))
            {
                try
                {
                    await client.LikeAsync(id, post.BasePost.ReblogKey);
                    post.Liked = true;
                }
                catch (Exception ex)
                {
                    await DisplayErrorMessage();
                }
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
                try
                {
                    await client.UnlikeAsync(id, post.BasePost.ReblogKey);
                    post.Liked = false;
                }
                catch (Exception ex)
                {
                    await DisplayErrorMessage();
                }
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
                    if (p.VideoUrl != null && p.VideoUrl != string.Empty)
                    {
                        bits.Add(new VideoBit(p.VideoUrl));
                    }
                    else
                    {
                        if(p.Player != null && p.Player.Any())
                        {
                            var vidPlayer = p.Player.Last();
                            var doc = new HtmlDocument();
                            doc.LoadHtml(vidPlayer.EmbedCode);

                            var youtubeElement = doc.DocumentNode.ChildNodes.First().GetAttributeValue("src", null);

                            bits.Add(new IFrameBit() { Url = youtubeElement });
                        }
                    }
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
                    var player = p.Player;
                    var xElement = XElement.Parse(player);
                    var source = xElement.Attribute("src").Value;

                    IFrameBit iframeBit = new IFrameBit() { Url = source };

                    var uri = new Uri(source);
                    if (uri.Host.EndsWith(".tumblr.com"))
                    {
                        iframeBit.Height = 85; // height of that player
                    }

                    bits.Add(iframeBit);
                    ParseTrails(bits, post.Trails);
                    break;

                case ChatPost _:
                case LinkPost _:
                case QuotePost _:
                    bits.Add(new NotImplementBit(post));
                    ParseTrails(bits, post.Trails);
                    break;
            }

            return new TumblrPost() { Author = post.BlogName, Bits = bits, Id = post.Id, NotesCount = post.NotesCount, RebloggedFrom = post.RebloggedFromName, Tags = post.Tags.ToList(), BasePost = post };
        }

        private IEnumerable<PostBit> ParseTextPost(TextPost p)
        {
            var body = p.Body;
            var title = p.Title;

            var bits = new List<PostBit>();

            List<Trail> trails = p.Trails;

            if (!trails.Any())
            {
                bits.Add(new HtmlTextBit($"<h1>{title}</h1>"));
                ParseTumblrHtml(bits, body);
            }
            else
            {
                if (title != null && title != string.Empty)
                {
                    trails[0].ContentRaw = $"<h1>{title}</h1>{trails[0].ContentRaw}";
                }
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

        private static bool AncestorsInclude(HtmlNode node, params string[] nodeTypes)
        {
            return node.AncestorsAndSelf().Any(t => nodeTypes.Contains(t.Name));
        }

        private static string GetLinkHref(HtmlNode node)
        {
            var aNode = node.AncestorsAndSelf().FirstOrDefault(t => t.Name == "a");
            if(aNode != null)
            {
                return aNode.GetAttributeValue("href", null);
            }

            return null;
        }

        private static void ParseTumblrHtml(List<PostBit> bits, string content)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);

            var documentNode = doc.DocumentNode;
            var nodesToInspect = new Stack<HtmlNode>();
            nodesToInspect.Push(documentNode);

            HtmlTextBit currentTextBit = new HtmlTextBit();

            while (nodesToInspect.Count > 0)
            {
                var node = nodesToInspect.Pop();

                if(node.Name == "li")
                {
                    currentTextBit.AddNewLine();
                    currentTextBit.AddString("  •  ");
                    foreach (var child in node.ChildNodes.Reverse())
                    {
                        nodesToInspect.Push(child);
                    }

                    continue;
                }

                if (node.Name == "p" || node.Name == "h1" || node.Name == "h2")
                {
                    currentTextBit.AddNewLine();
                    currentTextBit.AddNewLine();
                    foreach (var child in node.ChildNodes.Reverse())
                    {
                        nodesToInspect.Push(child);
                    }

                    continue;
                }

                if (node.Name == "#text")
                {
                    // some kind of text or link or something idk
                    var text = node.GetDirectInnerText();
                    if (text != null && text != "")
                    {
                        currentTextBit.AddString(text, 
                            GetLinkHref(node),
                            AncestorsInclude(node, "b", "strong"),
                            AncestorsInclude(node, "i", "em"),
                            AncestorsInclude(node, "strike"),
                            AncestorsInclude(node, "h2", "bigger"),
                            AncestorsInclude(node, "h1"),
                            AncestorsInclude(node, "blockquote", "pre")
                            );
                    }
                    continue;
                }

                if (node.Name == "img")
                {
                    ResetHtmlContainer();

                    var width = node.GetAttributeValue("data-orig-width", 100);
                    var height = node.GetAttributeValue("data-orig-height", 100);
                    bits.Add(new ImageBit(node.GetAttributeValue("src", ""), "text post image", new PhotoInfo() { Height = height, Width = width }));
                    continue;
                }

                if (node.Name == "figure")
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
                            ResetHtmlContainer();

                            bits.Add(new VideoBit(url));
                            continue;
                        }
                    }
                }

                if(node.Name == "iframe")
                {
                    ResetHtmlContainer();
                    bits.Add(new IFrameBit() { Url = node.GetAttributeValue("src", ".") });
                }

                foreach (var child in node.ChildNodes.Reverse())
                {
                    nodesToInspect.Push(child);
                }
            }

            ResetHtmlContainer();

            void ResetHtmlContainer()
            {
                currentTextBit.Spans = currentTextBit.Spans.SkipWhile(sp => string.IsNullOrWhiteSpace(sp.Text)).ToList();
                // anything else - images, video, something idk 
                // in that case we want to finish up the html collection
                if (currentTextBit.Spans.Count > 0)
                {
                    bits.Add(currentTextBit);
                    currentTextBit = new HtmlTextBit();
                }
            }
        }

        internal Task UnfollowBlog(string blog)
        {
            return this.client.UnfollowAsync(blog);
        }

        internal Task FollowBlog(string blog)
        {
            return this.client.FollowAsync(blog);
        }

        //content = content.Replace("<p>", "");
        //content = content.Replace("</p>", "<br />");

        //var doc = new HtmlAgilityPack.HtmlDocument();
        //doc.LoadHtml(content);

        //var documentNode = doc.DocumentNode;
        //var nodesToInspect = new Stack<HtmlNode>();
        //nodesToInspect.Push(documentNode);

        //while (nodesToInspect.Count > 0)
        //{
        //    var node = nodesToInspect.Pop();
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
        //    else if (node.Name == "a")
        //    {
        //        var text = node.GetDirectInnerText();
        //        if (text != null && text != "")
        //        {
        //            bits.Add(new HtmlTextBit() { html = "LINK:" + node.OuterHtml });
        //        }
        //    }
        //    else if (node.Name == "figure")
        //    {
        //        var npfData = node.GetAttributeValue("data-npf", "");
        //        if (npfData != null && npfData != "")
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
        //        else
        //        {
        //            foreach (var child in node.ChildNodes.Reverse())
        //            {
        //                nodesToInspect.Push(child);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var child in node.ChildNodes.Reverse())
        //        {
        //            nodesToInspect.Push(child);
        //        }
        //    }
        //}

        //return content;
        //}
    }
}

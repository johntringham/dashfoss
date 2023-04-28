using DashFoss.Models;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<TumblrPost>> GetPosts()
        {
            var posts = await client.GetDashboardPostsAsync();
            var parsed = posts.Select(p => ParsePost(p));

            return parsed;
        }

        private TumblrPost ParsePost(BasePost post)
        {
            var bits = new List<PostBit>();
            bits.Add(new HtmlTextBit() { html = $"{post.GetType().Name} - {post.BlogName} " });

            switch (post)
            {
                case AnswerPost p:
                    break;

                case AudioPost p:
                    break;

                case ChatPost p:
                    break;

                case LinkPost p:
                    break;

                case PhotoPost p:
                    foreach(var photo in p.PhotoSet)
                    {
                        bits.Add(new ImageBit() { Url = photo.OriginalSize.ImageUrl });
                    }
                    bits.Add(new HtmlTextBit() { html = p.Caption });

                    break;

                case QuotePost p:
                    break;

                case TextPost p:
                    bits.AddRange(ParseTextPost(p).ToList());
                    //bits.Add(new HtmlTextBit() { html = p.Body });
                    break;

                case VideoPost p:
                    break;
            }

            return new TumblrPost() { Author = post.BlogName, Content = "testing", Bits = bits };
        }

        private IEnumerable<PostBit> ParseTextPost(TextPost p)
        {
            var body = p.Body;
            var title = p.Title;

            var bits = new List<PostBit>();

            if (title != null && title.Length != 0)
            {
                bits.Add(new HtmlTextBit() { html = $"<h1>{title}</h1>" });
            }

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(body);
            var blockNodes = doc.DocumentNode.SelectNodes("//blockquote");

            if (blockNodes != null)
            {
                string previousPerson = null;
                foreach (var node in blockNodes)
                {
                    /*
                     * <p>
                            <a class="tumblr_blog" href="https://www.tumblr.com/blog/view/modosexo/712354533996724224">modosexo</a>:
                        </p>
                        <blockquote>
                            beef borger 
                        </blockquote>
                     * 
                     */

                    // html is old scool nesting, we want names before content
                    if (previousPerson != null)
                    {
                        var blogName = doc.CreateElement("p");
                        blogName.AddClass("RealBlogName");
                        blogName.InnerHtml = previousPerson;

                        node.ParentNode.InsertAfter(blogName, node);
                    }

                    // find the "tumblr_blog" class above to get who said this shit
                    var siblings = node.ParentNode.ChildNodes.Where(n => n.Name != "#text").ToList();
                    var indexOfThis = siblings.IndexOf(node);
                    var immeediatelyBefore = siblings[indexOfThis - 1];
                    var whoSaidThis = immeediatelyBefore.ChildNodes[0].InnerHtml; // shaky stuff here...
                    previousPerson = whoSaidThis;
                }
            }

            foreach(var node in doc.DocumentNode.Descendants())
            {
                if(node.Name != "p" && node.Name != "img")
                {
                    continue; // comp[letely pointlesss, just want to debug stuff easier. i don't want to bne stepping for days
                }

                if(node.Name == "p")
                {
                    if (node.HasClass("RealBlogName"))
                    {
                        bits.Add(new BlogNameBit() { BlogName = node.InnerHtml });
                    }
                    else
                    {
                        //if (!node.GetClasses().Any())
                        //{
                        if (node.ChildNodes.Any() && !node.ChildNodes[0].HasClass("tumblr_blog"))
                        {
                            bits.Add(new HtmlTextBit() { html = node.OuterHtml });
                        }
                        //}
                    }
                }

                if(node.Name == "img")
                {
                    bits.Add(new ImageBit() { Url = node.GetAttributeValue("src", "") });
                }
            }

            return bits;
        }
    }
}

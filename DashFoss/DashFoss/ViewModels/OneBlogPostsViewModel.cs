using DashFoss.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DashFoss.ViewModels
{
    public class OneBlogPostsViewModel : PostsViewModel
    {
        public string blog { get; set; }

        public OneBlogPostsViewModel() : base()
        {
            this.Title = blog;
        }

        public override async Task<IEnumerable<TumblrPost>> GetMostRecentPosts()
        {
            return await tumblrTalker.GetMostRecentPostsForAuthor(blog);
        }

        public override async Task<IEnumerable<TumblrPost>> GetOlderPosts(TumblrPost lastPost)
        {
            return await this.tumblrTalker.GetOlderPostsForAuthor(blog, this.Posts.Count);
        }
    }
}

using DashFoss.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DashFoss.ViewModels
{
    public class OneBlogPostsViewModel : PostsViewModel
    {
        private string _blog;

        public string blog
        {
            get => _blog; 
            set
            {
                _blog = value;
                this.Title = _blog;
            }
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

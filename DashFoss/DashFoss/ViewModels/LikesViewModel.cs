using DashFoss.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DashFoss.ViewModels
{
    public class LikesViewModel : PostsViewModel
    {
        public string blog { get; set; }

        public override async Task<IEnumerable<TumblrPost>> GetMostRecentPosts()
        {
            return await tumblrTalker.GetLikes();
        }

        public override async Task<IEnumerable<TumblrPost>> GetOlderPosts(TumblrPost lastPost)
        {
            return await this.tumblrTalker.GetOlderLikes(this.Posts.Count);
        }
    }
}

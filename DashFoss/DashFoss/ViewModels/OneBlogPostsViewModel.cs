using DashFoss.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DashFoss.ViewModels
{
    public class OneBlogPostsViewModel : PostsViewModel
    {
        private string _blog;
        private bool isFollowing;
        private string followText;

        public string blog
        {
            get => _blog; 
            set
            {
                _blog = value;
                this.Title = _blog;
            }
        }

        public bool IsFollowing { get => isFollowing; set => SetProperty(ref isFollowing, value); }
        public string FollowButtonText { get => followText; set => SetProperty(ref followText, value); }

        public OneBlogPostsViewModel()
        {
            this.ShouldShowFollowButton = true;
            this.FollowUnfollowButtonCommand = new Command(async () => await this.FollowUnfollow());
        }

        private async Task FollowUnfollow()
        {
            FollowButtonText = "⏳";

            if (await tumblrTalker.DoesFollow(blog))
            {
                await tumblrTalker.UnfollowBlog(blog);
            }
            else
            {
                await tumblrTalker.FollowBlog(blog);
            }

            await UpdateFollowButtonText();
        }

        public override async Task<IEnumerable<TumblrPost>> GetMostRecentPosts()
        {
            await UpdateFollowButtonText();

            return await tumblrTalker.GetMostRecentPostsForAuthor(blog);
        }

        private async Task UpdateFollowButtonText()
        {
            if (await tumblrTalker.DoesFollow(blog))
            {
                this.IsFollowing = true;
                FollowButtonText = "Unfollow " + blog;
            }
            else
            {
                this.IsFollowing = false;
                FollowButtonText = "Follow " + blog;
            }
        }

        public override async Task<IEnumerable<TumblrPost>> GetOlderPosts(TumblrPost lastPost)
        {
            return await this.tumblrTalker.GetOlderPostsForAuthor(blog, this.Posts.Count);
        }

        public Command FollowUnfollowButtonCommand { get; set; }
    }
}

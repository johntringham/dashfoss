using DashFoss.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DashFoss.Services
{
    public class TumblrDashboardPostsDataStore : IDataStore<TumblrPost>
    {

        List<TumblrPost> posts;

        public TumblrDashboardPostsDataStore()
        {
            this.posts = new List<TumblrPost>()
            {
                new TumblrPost() { Content = "hi", Author="greg" },
                new TumblrPost() { Content = "hi", Author="greg" },
                new TumblrPost() { Content = "hi", Author="greg" },
            };
        }
        
        public Task<bool> AddItemAsync(TumblrPost item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TumblrPost> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TumblrPost>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(this.posts);
        }

        public Task<bool> UpdateItemAsync(TumblrPost item)
        {
            throw new NotImplementedException();
        }
    }
}

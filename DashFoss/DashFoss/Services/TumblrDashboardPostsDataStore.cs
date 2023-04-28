using DashFoss.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DashFoss.Services
{
    //public class TumblrDashboardPostsDataStore : IDataStore<TumblrPost>
    //{
    //    private readonly TumblrTalker talker;
    //    List<TumblrPost> posts;

    //    public TumblrDashboardPostsDataStore()
    //    {
    //        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

    //        this.talker = DependencyService.Get<TumblrTalker>();

    //        //this.posts = new List<TumblrPost>()
    //        //{
    //        //    new TumblrPost() { Content = "hi", Author="greg" },
    //        //    new TumblrPost() { Content = "hi", Author="greg" },
    //        //    new TumblrPost() { Content = "hi", Author="greg" },
    //        //};
    //    }
        
    //    public Task<bool> AddItemAsync(TumblrPost item)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> DeleteItemAsync(string id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<TumblrPost> GetItemAsync(string id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<IEnumerable<TumblrPost>> GetItemsAsync(bool forceRefresh = false, long since = -1l)
    //    {
    //        //return await Task.FromResult(this.posts);

    //        var posts = await talker.GetPosts(since);
    //        return posts;
    //    }


    //    public Task<bool> UpdateItemAsync(TumblrPost item)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

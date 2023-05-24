using Flurl.Http;
using Flurl;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace TumblrSharp.Simple
{
    public static class TumblrClient
    {
        #region Static Methods

        #region GetAllAsync
        /// <summary>
        /// Get the entire blog, including 
        /// </summary>
        public static Task<BlogRoot> GetAllAsync(
            string blog, 
            int? offset = null, 
            int? count = null,
            PostType type = PostType.All,
            Filter filter = Filter.None)
        {
            return GetAllBaseAsync(blog, offset, count, type, filter);
        }

        async public static Task<BlogRoot> GetAllAsync(string blog, long id, Filter filter = Filter.None)
        {
            return await GetAllBaseAsync(blog: blog, id: id, filter: filter);
        }

        /// <summary>
        /// Generic private method to be used by either overloaded method
        /// </summary>
        /// <param name="blog"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <param name="filter"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        async private static Task<BlogRoot> GetAllBaseAsync(
            string blog,
            int? offset = null,
            int? count = null,
            PostType type = PostType.All,
            Filter filter = Filter.None,
            long? id = null)
        {
            if (string.IsNullOrEmpty(blog))
                throw new ArgumentNullException("blog");

            // build our request
            var request = blog
                .GenerateUrl();

            // add parameters
            if (offset.HasValue)
                request = request.SetQueryParam("start", offset.Value);
            if (count.HasValue)
                request = request.SetQueryParam("num", count.Value);
            if (type != PostType.All)
                request = request.SetQueryParam("type", type.ToString().ToLower());
            if (filter != Filter.None)
            {
                string useFilter = "";
                switch (filter)
                {
                    case Filter.Raw:
                        useFilter = "none";
                        break;
                    case Filter.Text:
                        useFilter = "text";
                        break;
                }
                request = request.SetQueryParam("filter", useFilter);
            }
            if (id.HasValue)
                request = request.SetQueryParam("id", id.Value);

            // get our data
            var json = await request.GetStringAsync();
                
            // convert JSON using our custom reader
            return await Task.Factory.StartNew(() =>
            {
                var result = JsonConvert.DeserializeObject<BlogRoot>(json.StripJS(), new PostArrayConverter());

                // make our total posts accessible in the Blog instance
                result.Blog.TotalPosts = result.TotalPosts;

                return result;
            });
        }
        #endregion

        #region GetPostsAsync
        /// <summary>
        /// Get posts of a particular type from a blog.
        /// </summary>
        /// <typeparam name="T">The type of post to get.  Can be any class that inherits from <see cref="BasePost"/>.</typeparam>
        /// <param name="blog">The name of the blog to get posts from.</param>
        /// <param name="offset">The offset of the first post.</param>
        /// <param name="count">The amount of posts to get.</param>
        /// <param name="filter">The filter for posts.</param>
        /// <returns></returns>
        async public static Task<T[]> GetPostsAsync<T>(string blog, int? offset = null, int? count = null, Filter filter = Filter.None)
        {
            var basePost = await GetAllAsync(blog, offset, count, ConvertObjectToPostType(typeof(T)), filter);

            // automatically cast if a post type is specified
            List<T> result = new List<T>();
            foreach (var item in basePost.Posts)
            {
                result.Add((T)(object)item);
            }
            return result.ToArray();            
        }

        /// <summary>
        /// Get posts from a blog.
        /// </summary>
        /// <param name="blog">The name of the blog to get posts from.</param>
        /// <param name="type">The optional type of post to get.</param>
        /// <param name="offset">The offset of the first post.</param>
        /// <param name="count">The amount of posts to get.</param>
        /// <param name="filter">The filter for posts.</param>
        /// <returns></returns>
        async public static Task<BasePost[]> GetPostsAsync(string blog, PostType type = PostType.All, int? offset = null, int? count = null, Filter filter = Filter.None)
        {
            var basePost = await GetAllAsync(blog, offset, count, type, filter);
            return basePost.Posts;
        }

        /// <summary>
        /// Get a specific post from a specified blog.
        /// </summary>
        /// <param name="blog">The name of the blog.</param>
        /// <param name="id">The ID of the post.</param>
        /// <param name="filter">The filter to implement.</param>
        /// <returns></returns>
        async public static Task<BasePost> GetPostAsync(string blog, long id, Filter filter = Filter.None)
        {
            var basePost = await GetAllAsync(blog, id: id, filter: filter);
            if (basePost.Posts.Length > 0)
                return basePost.Posts[0];
            else
                return null;
        }
        #endregion

        #region GetBlogInfoAsync

        /// <summary>
        /// Get all info pertaining to a blog.
        /// </summary>
        /// <param name="blog">The name of the blog.</param>
        /// <returns></returns>
        async public static Task<Blog> GetBlogInfoAsync(string blog)
        {
            var basePost = await GetAllAsync(blog: blog, count: 1);
            return basePost.Blog;
        }
        #endregion

        #endregion

        #region Helper Method Extensions
        /// <summary>
        /// Get the API endpoing from a blog name.
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public static string GenerateUrl(this string blog)
        {
            return $"http://{blog}.tumblr.com/api/read/json";
        }

        /// <summary>
        /// because Tumblr gives us JS back, and not JSON, we need to strip it.
        /// we can do this relatively safely with a regex that deletes everything before the first "{"
        /// and the final character (a semicolon).  However, an even quicker way would to simply perform
        /// a substring, since Tumblr always adds "var tumblr_api_read = ", a total of 22 chars (index starting at 21)
        /// and one ";" at the end (although since we are performing a substring, it will be 1 + start index (21), or 22.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string StripJS(this string json)
        {
            return json.Substring(21, json.Length - 22);
        }
        #endregion

        public enum PostType
        {
            All,
            Text,
            Quote,
            Photo,
            Link,
            Chat,
            Video,
            Audio
        }

        public enum Filter
        {
            None,
            Raw,
            Text
        }

        private static PostType ConvertObjectToPostType(Type type) {

            if (type == typeof(AudioPost))
                return PostType.Audio;
            if (type == typeof(ChatPost))
                return PostType.Chat;
            if (type == typeof(LinkPost))
                return PostType.Link;
            if (type == typeof(QuotePost))
                return PostType.Quote;
            if (type == typeof(TextPost))
                return PostType.Text;
            if (type == typeof(VideoPost))
                return PostType.Video;

            return PostType.All;
        }
    }
}

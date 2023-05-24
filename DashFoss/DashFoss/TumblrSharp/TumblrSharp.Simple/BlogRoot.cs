using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    /// <summary>
    /// Contains all data for a blog page
    /// </summary>
    public class BlogRoot
    {
        /// <summary>
        /// All the information pertaining to the userblog of this account.
        /// </summary>
        [JsonProperty(PropertyName = "tumblelog")]
        public Blog Blog { get; set; }

        /// <summary>
        /// The offset of the first post in this set.
        /// </summary>
        [JsonProperty(PropertyName = "posts-start")]
        public long PostOffset { get; set; }

        /// <summary>
        /// The total amount of posts from this Tumblr.
        /// </summary>
        [JsonProperty(PropertyName = "posts-total")]
        public long TotalPosts { get; set; }

        /// <summary>
        /// All the posts from this page.
        /// </summary>
        [JsonConverter(typeof(PostArrayConverter))]
        [JsonProperty(PropertyName = "posts")]
        public BasePost[] Posts { get; set; }
    }
}
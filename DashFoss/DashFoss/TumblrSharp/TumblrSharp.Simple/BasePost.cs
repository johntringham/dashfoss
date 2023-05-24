using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumblrSharp.Simple
{
    public class BasePost
    {
        /// <summary>
		/// The url of the post.
		/// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; private set; }

        /// <summary>
		/// The slug of the post.
		/// </summary>
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; private set; }

        /// <summary>
		/// The url of the post with optional slug.
		/// </summary>
        [JsonProperty(PropertyName = "url-with-slug")]
        public string UrlSlug { get; private set; }

        /// <summary>
		/// The date of the post, from the blog's timezone.
		/// </summary>
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; private set; }

        /// <summary>
		/// The date of the post, from GMT.
		/// </summary>
        [JsonProperty(PropertyName = "date-gmt")]
        public DateTime DateGMT { get; private set; }

        /// <summary>
		/// The datetime of the post represented as a unix timestamp.
		/// </summary>
        [JsonProperty(PropertyName = "unix-timestamp")]
        public long Timestamp { get; private set; }

        /// <summary>
		/// The key needed to reblog the post.
		/// </summary>
        [JsonProperty(PropertyName = "reblog-key")]
        public string ReblogKey { get; private set; }

        /// <summary>
		/// The unique ID of this post.
		/// </summary>
        [JsonProperty(PropertyName = "id")]
        public long Id { get; private set; }

        /// <summary>
		/// The tags for this post, if any.
		/// </summary>
        [JsonProperty(PropertyName = "tags")]
        public string[] Tags { get; private set; }

    }
}

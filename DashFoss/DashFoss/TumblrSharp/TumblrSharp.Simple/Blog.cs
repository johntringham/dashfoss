using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class Blog
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; private set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; private set; }

        [JsonProperty(PropertyName = "cname")]
        public string Domain { get; private set; }

        /// <summary>
        /// The amount of posts this user has made.
        /// </summary>
        public long TotalPosts { get; set; }
    }
}
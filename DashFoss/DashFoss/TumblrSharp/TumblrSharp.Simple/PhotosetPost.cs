using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class PhotosetPost : BasePost
    {
        /// <summary>
		/// Caption of the post.
		/// </summary>
        [JsonProperty(PropertyName = "photo-caption")]
        public string Caption { get; set; }

        /// <summary>
		/// All photos in this photoset.
		/// </summary>
        [JsonProperty(PropertyName = "photos")]
        public Photo[] Photos { get; set; }
    }
}
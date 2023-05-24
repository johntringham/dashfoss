using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class LinkPost : BasePost
    {
        /// <summary>
		/// The title of this link.
		/// </summary>
        [JsonProperty(PropertyName = "link-text")]
        public string Title { get; private set; }

        /// <summary>
		/// The redirect URL of this link.
		/// </summary>
        [JsonProperty(PropertyName = "link-url")]
        public string Link { get; private set; }

        /// <summary>
		/// The optional description of this link.
		/// </summary>
        [JsonProperty(PropertyName = "link-description")]
        public string Description { get; private set; }
    }
}
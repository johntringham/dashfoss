using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class TextPost : BasePost
    {
        /// <summary>
		/// The title of the text post, if any.
		/// </summary>
        [JsonProperty(PropertyName = "regular-title")]
        public string Title { get; private set; }

        /// <summary>
		/// The body of the text post, if any.
		/// </summary>
        [JsonProperty(PropertyName = "regular-body")]
        public string Body { get; private set; }
    }
}
using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class QuotePost : BasePost
    {
        /// <summary>
		/// The quote of the post.
		/// </summary>
        [JsonProperty(PropertyName = "quote-text")]
        public string Quote { get; private set; }

        /// <summary>
		/// The text source of the quote.
		/// </summary>
        [JsonProperty(PropertyName = "quote-source")]
        public string Source { get; private set; }
    }
}
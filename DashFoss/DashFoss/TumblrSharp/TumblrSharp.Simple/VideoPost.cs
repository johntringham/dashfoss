using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class VideoPost : BasePost
    {
        /// <summary>
		/// The caption of the video post, if any.
		/// </summary>
        [JsonProperty(PropertyName = "video-caption")]
        public string Body { get; private set; }

        /// <summary>
		/// The source (link) of this video.
		/// </summary>
        [JsonProperty(PropertyName = "video-source")]
        public string Source { get; private set; }

        /// <summary>
		/// The HTML embed of this video.
		/// </summary>
        [JsonProperty(PropertyName = "video-player")]
        public string Embed { get; private set; }
    }
}
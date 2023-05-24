using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class AudioPost : BasePost
    {
        /// <summary>
		/// The artist of this track, if specified.
		/// </summary>
        [JsonProperty(PropertyName = "id3-artist")]
        public string Artist { get; private set; }

        /// <summary>
		/// The album of this track, if specified.
		/// </summary>
        [JsonProperty(PropertyName = "id3-album")]
        public string Album { get; private set; }

        /// <summary>
		/// The title of this track, if specified.
		/// </summary>
        [JsonProperty(PropertyName = "id3-title")]
        public string Title { get; private set; }

        /// <summary>
		/// The optional caption of this post.
		/// </summary>
        [JsonProperty(PropertyName = "audio-caption")]
        public string Caption { get; private set; }

        /// <summary>
		/// The HTML audio player of this post.
		/// </summary>
        [JsonProperty(PropertyName = "audio-player")]
        public string Player { get; private set; }

        /// <summary>
		/// The HTML embed code of this post.
		/// </summary>
        [JsonProperty(PropertyName = "audio-embed")]
        public string Embed { get; private set; }

        /// <summary>
		/// The amount of plays this track has recieved.
		/// </summary>
        [JsonProperty(PropertyName = "audio-plays")]
        public int PlayCount { get; private set; }
    }
}
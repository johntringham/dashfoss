using Newtonsoft.Json;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// Contains a post's notes.
    /// </summary>
    public class Notes
    {
        /// <summary>
        /// Total number of liked posts.
        /// </summary>
        [JsonProperty(PropertyName = "total_notes")]
        public long Count { get; set; }

        /// <summary>
        /// An array of <see cref="BasePost"/> instances, representing
        /// the liked posts.
        /// </summary>
        [JsonConverter(typeof(NotesConverter))]
        [JsonProperty(PropertyName = "notes")]
        public BaseNote[] Result { get; set; }
    }
}

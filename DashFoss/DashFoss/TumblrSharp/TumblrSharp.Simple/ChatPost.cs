using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class ChatPost : BasePost
    {
        /// <summary>
		/// The title of the chat.
		/// </summary>
        [JsonProperty(PropertyName = "conversation-title")]
        public string Title { get; private set; }

        /// <summary>
		/// The raw text of the post.  Use the Chat property to get a more structured result.
		/// </summary>
        [JsonProperty(PropertyName = "conversation-text")]
        public string Text { get; private set; }

        /// <summary>
		/// Each line of the chat in a structured format.
		/// </summary>
        [JsonProperty(PropertyName = "conversation")]
        public ChatLine[] Chat { get; private set; }
    }

    public class ChatLine
    {
        /// <summary>
        /// The name of the speaker.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// The label before the speaker's text.  For instance, if the name were "doomy", the label would be "doomy:".
        /// </summary>
        [JsonProperty(PropertyName = "label")]
        public string Label { get; private set; }

        /// <summary>
        /// The body of the speaker's text.
        /// </summary>
        [JsonProperty(PropertyName = "phrase")]
        public string Body { get; private set; }
    }
}
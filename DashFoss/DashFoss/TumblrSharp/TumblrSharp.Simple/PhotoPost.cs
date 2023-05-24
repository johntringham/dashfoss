using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TumblrSharp.Simple
{
    public class PhotoPost : BasePost
    {
        /// <summary>
		/// Caption of the post.
		/// </summary>
        [JsonProperty(PropertyName = "photo-caption")]
        public string Caption { get; private set; }


        [JsonProperty(PropertyName = "photo-url-1280")]
        private string Size1280 { get; set; }

        [JsonProperty(PropertyName = "photo-url-500")]
        private string Size500 { get; set; }

        [JsonProperty(PropertyName = "photo-url-400")]
        private string Size400 { get; set; }

        [JsonProperty(PropertyName = "photo-url-250")]
        private string Size250 { get; set; }

        [JsonProperty(PropertyName = "photo-url-100")]
        private string Size100 { get; set; }

        [JsonProperty(PropertyName = "photo-url-75")]
        private string Size75 { get; set; }

        // tidy everything neatly into one object
        public Photo Photo { get; private set; }

        [OnDeserialized]
        public void SetPhotoObject(StreamingContext context)
        {
            Photo = new Photo(Size75, Size100, Size250, Size400, Size500, Size1280);
        }
    }
}
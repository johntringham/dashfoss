using Newtonsoft.Json;

namespace TumblrSharp.Simple
{
    public class Photo
    {
        /// <summary>
		/// Caption of the photo, if any.
		/// </summary>
        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; private set; }

        /// <summary>
		/// 1280px wide version of this image.
		/// </summary>
        [JsonProperty(PropertyName = "photo-url-1280")]
        public string Size1280 { get; private set; }

        /// <summary>
		/// 500px wide version of this image.
		/// </summary>
        [JsonProperty(PropertyName = "photo-url-500")]
        public string Size500 { get; private set; }

        /// <summary>
		/// 400px wide version of this image.
		/// </summary>
        [JsonProperty(PropertyName = "photo-url-400")]
        public string Size400 { get; private set; }

        /// <summary>
		/// 250px wide version of this image.
		/// </summary>
        [JsonProperty(PropertyName = "photo-url-250")]
        public string Size250 { get; private set; }

        /// <summary>
		/// 100px wide version of this image.
		/// </summary>
        [JsonProperty(PropertyName = "photo-url-100")]
        public string Size100 { get; private set; }

        /// <summary>
		/// 75px wide version of this image.
		/// </summary>
        [JsonProperty(PropertyName = "photo-url-75")]
        public string Size75 { get; private set; }

        public Photo() { }

        /// <summary>
        /// Use this constructor when using a PhotoPost object to consolidate all properties into one object.
        /// </summary>
        /// <param name="size75"></param>
        /// <param name="size100"></param>
        /// <param name="size250"></param>
        /// <param name="size400"></param>
        /// <param name="size500"></param>
        /// <param name="size1280"></param>
        public Photo(string size75, string size100, string size250, string size400, string size500, string size1280)
        {
            Size75 = size75;
            Size100 = size100;
            Size250 = size250;
            Size400 = size400;
            Size500 = size500;
            Size1280 = size1280;
        }

        /// <summary>
        /// Get an image whose width is greater than or equal to the specified amount.
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public string GetClosestTo(int width)
        {
            if (width <= 75)
                return Size75;

            if (width <= 100)
                return Size100;

            if (width <= 250)
                return Size250;

            if (width <= 400)
                return Size400;

            if (width <= 500)
                return Size500;

            if (width <= 1280)
                return Size1280;

            return Size1280;
        }
    }
}
using DashFoss.ViewModels;
using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFoss.Models
{
    public class TumblrPost : BaseViewModel
    {
        public string Author { get; set; }

        public string Id { get; set; }

        public long NotesCount { get; set; }

        public List<PostBit> Bits { get; set; }

        public string RebloggedFrom { get; set; }   

        public bool WasReblogged => RebloggedFrom != null && RebloggedFrom != string.Empty;

        public string AvatarUrl => $"https://api.tumblr.com/v2/blog/{Author}/avatar/48";

        public BasePost BasePost { get; set; }

        public string PostUrl => BasePost.Url;

        public List<string> Tags { get; set; }

        public string AllTags => string.Join(" ", Tags.Select(t => "#" + t));

        public bool GotTags => Tags.Count > 0;

        public List<BaseNote> Replies { get; set; } = new List<BaseNote>();
        public List<BaseNote> ReblogNotes { get; set; } = new List<BaseNote>();

        public bool Liked
        { 
            get
            {
                return BasePost.Liked == "True"; // ???
            }
            set
            {
                BasePost.Liked = value ? "True" : "False";
                OnPropertyChanged(nameof(Liked));
            }
        }
    }
}

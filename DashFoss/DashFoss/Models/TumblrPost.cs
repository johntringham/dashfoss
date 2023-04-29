using System;
using System.Collections.Generic;
using System.Text;

namespace DashFoss.Models
{
    public class TumblrPost
    {
        public string Author { get; set; }

        public long Id { get; set; }

        public long Notes { get; set; }

        public List<PostBit> Bits { get; set; }

        public string RebloggedFrom { get; set; }
    }
}

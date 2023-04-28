using System;
using System.Collections.Generic;
using System.Text;

namespace DashFoss.Models
{
    public class TumblrPost
    {
        public string Author { get; set; }

        public long Id { get; set; }

        public List<PostBit> Bits { get; set; }
    }
}

using DashFoss.Models;
using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DashFoss.ViewModels
{
    internal class NotesViewModel : BaseViewModel
    {
        public TumblrPost Post { get; set; }

        public List<BaseNote> Notes => this.Post.Notes;
    }
}

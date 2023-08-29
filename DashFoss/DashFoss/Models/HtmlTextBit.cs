using DashFoss.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.CommunityToolkit.Markup;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DashFoss.Models
{
    public class HtmlTextBit : PostBit
    {
        //public string html { get; set; }

        private static InlineLinkClickCommand LinkOpeningCommand = new InlineLinkClickCommand();

        public HtmlTextBit(params Span[] spans)
        {
            this.Spans.AddRange(spans);
        }

        public HtmlTextBit(string basicString)
        {
            this.Spans.Add(new Span() {  Text = basicString });
        }

        public void AddString(string str, string href = null, bool bold = false, bool italic = false, bool strike = false, bool h2 = false, bool h1=false, bool quote = false)
        {
            var span = new Span() { Text = str, FontSize = 16 };

            if(href != null)
            {
                span.ForegroundColor = (Color)Xamarin.Forms.Application.Current.Resources["LinkTextColor"];
                span.TextDecorations |= TextDecorations.Underline;

                span.GestureRecognizers.Add(new TapGestureRecognizer() { Command = LinkOpeningCommand, CommandParameter = href });
            }

            if (bold)
            {
                span.FontAttributes |= FontAttributes.Bold;
            }

            if(italic || quote)
            {
                span.FontAttributes |= FontAttributes.Italic;
            }

            if (strike)
            {
                span.TextDecorations |= TextDecorations.Strikethrough;
            }

            if (h1)
            {
                span.FontSize *= 2.1d;
            }

            if (h2)
            {
                span.FontSize *= 1.6d;
            }

            if (quote)
            {
                // todo: this is a bit rubbish. this fake parser can't cope well with this for blockquotes.
                // better soloution is to put blockquotes as a different HtmlTextBit, and not bundle them in with the other formatting stuff
                // can't do indenting this way because any margin/padding changes would be applied to all spans inside the blockquote, which is not what we want
                span.FontSize *= 0.9d;
                span.LineHeight *= 1.3d;
                span.TextColor = Color.AliceBlue;
            }


            this.Spans.Add(span);
        }

        internal void AddNewLine()
        {
            this.Spans.Add(new Span() { Text= Environment.NewLine });
        }

        public List<Span> Spans { get; set; } = new List<Span>();

        public FormattedString FormattedString 
        {
            get
            {
                var lastSpan = this.Spans.Last();
                lastSpan.Text = lastSpan.Text + "  "; // hack - nbsp

                // todo: check if this gets hit loads. if so then we need a way of only doing this once rather than recomputing
                var formattedString = new FormattedString();
                foreach(var span in Spans)
                {
                    formattedString.Spans.Add(span);
                }

                return formattedString;
            }
        }
    }

}

using DashFoss.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DashFoss.XamlBits
{
    public class PostBitDataTemplateSelector : DataTemplateSelector
    {

        public DataTemplate HtmlTextBitTemplate { get; set; }
        public DataTemplate ImageBitTemplate { get; set; }
        public DataTemplate BlogNameTextBitTemplate { get; set; }
        public DataTemplate VideoBitTemplate { get; set; }
        public DataTemplate QABitTemplate { get; set; }
        public DataTemplate IFrameBitTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case HtmlTextBit _: return HtmlTextBitTemplate;
                case ImageBit _: return ImageBitTemplate;
                case BlogNameBit _: return BlogNameTextBitTemplate;
                case VideoBit _: return VideoBitTemplate;
                case QuestionAnswerBit _: return QABitTemplate;
                case IFrameBit _: return IFrameBitTemplate;
            }

            throw new InvalidOperationException("no template for that kind  of bit");
        }
    }
}

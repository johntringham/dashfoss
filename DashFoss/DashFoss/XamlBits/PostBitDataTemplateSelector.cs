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

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case HtmlTextBit _: return HtmlTextBitTemplate;
                case ImageBit _: return ImageBitTemplate;
                case BlogNameBit _: return BlogNameTextBitTemplate;
            }

            throw new InvalidOperationException("no template for that kind  of bit");
        }
    }
}

using System.Reflection;
using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Svg;

namespace DashFoss.XamlBits
{
    [ContentProperty(nameof(Source))]
    public class CustomImageSourceExtension : IMarkupExtension<ImageSource>, IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return GetValue();
        }

        ImageSource IMarkupExtension<ImageSource>.ProvideValue(IServiceProvider serviceProvider)
        {
            return GetValue();
        }

        private ImageSource GetValue()
        {
            if (Source == null)
            {
                return null;
            }

            if (!Source.StartsWith("DashFoss"))
            {
                Source = "DashFoss.Resources.Images." + Source;
            }

            if (Source.ToLowerInvariant().EndsWith(".svg"))
            {
                return SvgImageSource.FromSvgResource(Source);
            }

            var imageSource = ImageSource.FromResource(Source, typeof(CustomImageSourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ImageFillWidthForms
{
    public class AspectRatioContainer : ContentView
    {
        public PhotoInfo PhotoInfo
        {
            get { return (PhotoInfo)GetValue(PhotoInfoProperty); }
            set { SetValue(PhotoInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PhotoInfo.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty PhotoInfoProperty =
            BindableProperty.Create(nameof(PhotoInfo), typeof(PhotoInfo), typeof(AspectRatioContainer), null, propertyChanged: OnPhotoInfoChanged);

        private static void OnPhotoInfoChanged(BindableObject bindable, object oldValue, object newValue)
        {
            AspectRatioContainer aspectRatioContainer = (bindable as AspectRatioContainer);
            aspectRatioContainer.AspectRatio = ((double)aspectRatioContainer.PhotoInfo.Height) / (double)(aspectRatioContainer.PhotoInfo.Width);
        }

        public static BindableProperty AspectRatioProperty = BindableProperty.Create(nameof(AspectRatio), typeof(double), typeof(AspectRatioContainer), (double)1);

        public double AspectRatio
        {
            get { return (double) this.GetValue(AspectRatioProperty); }
            set
            {
                this.SetValue(AspectRatioProperty, value);
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint * this.AspectRatio));
        }
    }
}

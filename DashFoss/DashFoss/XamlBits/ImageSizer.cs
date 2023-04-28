using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DashFoss.XamlBits
{
    public class ImageSizer : Image
    {


        public PhotoInfo Photo
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(ownerclass), new PropertyMetadata(0));



        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint, heightConstraint);
        }
    }
}

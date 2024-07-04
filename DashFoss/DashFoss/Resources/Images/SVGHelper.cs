using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Svg;

namespace DashFoss
{
    public class SVGHelper
    {
        public static ImageSource GetSVG(string name)
        {
            return SvgImageSource.FromSvgResource("DashFoss.Resources.Images." + name);
        }
    }
}

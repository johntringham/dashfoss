using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Svg;

namespace DashFoss.Commands
{
    public class PauseVideoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var mediaElement = parameter as MediaElement;
            if (mediaElement != null)
            {
                if (mediaElement.CurrentState == MediaElementState.Playing)
                {
                    mediaElement.Pause();
                }
                else
                {
                    mediaElement.Play();
                }
            }
        }
    }

    public class MuteVideoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var mediaElement = parameter as MediaElement;
            if (mediaElement != null)
            {
                if (mediaElement.Volume == 0)
                {
                    mediaElement.Volume = 1.0;
                }
                else
                {
                    mediaElement.Volume = 0.0;
                }
            }
        }
    }

    public class RewindVideoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var mediaElement = parameter as MediaElement;
            mediaElement.Position = TimeSpan.Zero;
        }
    }

    public class IsMutedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var volume = (double)value;
            if (volume == 0)
            {
                return SVGHelper.GetSVG("volumex.svg");
            }
            else
            {
                return SVGHelper.GetSVG("volume2.svg");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsPlayingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (MediaElementState)value;
            if (state == MediaElementState.Playing)
            {
                return SVGHelper.GetSVG("pause.svg");
            }
            else
            {
                return SVGHelper.GetSVG("play.svg");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

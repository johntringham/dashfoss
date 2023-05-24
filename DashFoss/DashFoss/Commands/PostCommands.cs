using DashFoss.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace DashFoss.Commands
{
    public class LikePostCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var post = parameter as TumblrPost;
            if (post != null)
            {
                post.Liked = !post.Liked;

                //if (post.Liked)
                //{
                //    post.Unlike();
                //}
                //else
                //{
                //    post.Like();
                //}
            }
        }
    }

    public class IsLikedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var post = parameter as TumblrPost;
            if(post != null && post.Liked)
            {
                return "❤";
            }
            else
            {
                return "🤍";
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

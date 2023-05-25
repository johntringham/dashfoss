using DashFoss.Models;
using DashFoss.Services;
using DashFoss.ViewModels;
using DashFoss.Views;
using Flurl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
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

        public async void Execute(object parameter)
        {
            var post = parameter as TumblrPost;
            if (post != null)
            {
                var desiredLike = !post.Liked;

                var tumblrTalker = DependencyService.Get<TumblrTalker>();

                if (desiredLike)
                {
                    await tumblrTalker.DoLike(post);
                }
                else
                {
                    await tumblrTalker.DoUnlike(post);
                }
            }
        }
    }

    public class IsLikedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var liked = (bool)value;
            if (liked)
            {
                return "❤";
            }
            else
            {
                return "🤍";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InlineLinkClickCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var url = (string)parameter;
            await Launcher.OpenAsync(url);
        }
    }

    public class OpenBlogCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var page = new ItemsPage();
            page.BindingContext = new OneBlogPostsViewModel() { blog = (string) parameter };
            
            Shell.Current.Navigation.PushAsync(page);
        }
    }
}

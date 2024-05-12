using DashFoss.Services;
using DashFoss.Views;
using DontPanic.TumblrSharp.OAuth;
using Flurl.Util;
using Flurl;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DashFoss.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        private const string authorizeUrl = "www.tumblr.com/oauth2/authorize";

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            this.ApiConsumer = Keys.CONSUMER_KEY;
            this.ApiSecret = Keys.CONSUMER_SECRET;
        }

        private async void OnLoginClicked(object obj)
        {
            Token token;

            var queryItems = new Dictionary<string, string>()
            {
                ["client_id"] = this.ApiConsumer,
                ["response_type"] = "code",
                ["scope"] = "write offline_access",
                ["state"] = "some state here",
            };

            Url url = authorizeUrl.SetQueryParams(queryItems.ToKeyValuePairs());
            await Launcher.OpenAsync("https://" + url.ToString());
        }

        public string ApiConsumer { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string ApiSecret { get; set; }
    }
}

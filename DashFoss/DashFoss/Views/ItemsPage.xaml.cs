using DashFoss.Models;
using DashFoss.ViewModels;
using DashFoss.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DashFoss.Views
{
    public partial class ItemsPage : ContentPage
    {
        PostsViewModel _viewModel => this.BindingContext as PostsViewModel;

        public ItemsPage()
        {
            InitializeComponent();

            //BindingContext = _viewModel = new PostsViewModel();

            ItemsListView.Scrolled += Scrolled;
        }

        private void Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if(_viewModel.Posts.Count > 0 && e.LastVisibleItemIndex > _viewModel.Posts.Count - 2)
            {
                _viewModel.LoadMorePosts();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
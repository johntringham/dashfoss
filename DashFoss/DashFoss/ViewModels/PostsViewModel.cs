using DashFoss.Models;
using DashFoss.Services;
using DashFoss.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DashFoss.ViewModels
{
    public class PostsViewModel : BaseViewModel
    {
        private Item _selectedItem;
        public ObservableCollection<TumblrPost> Posts { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }

        public TumblrTalker tumblrTalker;

        public PostsViewModel()
        {
            Title = "Browse";

            Posts = new ObservableCollection<TumblrPost>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            tumblrTalker = new TumblrTalker();
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Posts.Clear();
                IEnumerable<TumblrPost> items = await GetMostRecentPosts();
                foreach (var item in items)
                {
                    Posts.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public virtual async Task<IEnumerable<TumblrPost>> GetMostRecentPosts()
        {
            return await tumblrTalker.GetMostRecentPosts();
        }

        public virtual async Task<IEnumerable<TumblrPost>> GetOlderPosts(TumblrPost lastPost)
        {
            return await this.tumblrTalker.GetOlderPosts(lastPost.Id);
        }

        public void OnAppearing()
        {
            if(this.Posts.Count == 0)
            {
                IsBusy = true;
                SelectedItem = null;
            }
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        bool loadingMorePosts = false;
        internal async void LoadMorePosts()
        {
            if (loadingMorePosts) return;

            loadingMorePosts = true;

            var lastPost = this.Posts.Last();
            IEnumerable<TumblrPost> morePosts = await GetOlderPosts(lastPost);
            foreach (var post in morePosts)
            {
                this.Posts.Add(post);
            }

            loadingMorePosts = false;
        }

        
    }
}
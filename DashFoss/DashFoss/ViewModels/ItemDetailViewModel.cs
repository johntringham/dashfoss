using DashFoss.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DashFoss.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string content;
        private string description;
        public string Id { get; set; }

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }

        //public string Description
        //{
        //    get => description;
        //    set => SetProperty(ref description, value);
        //}

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            
        }
    }
}

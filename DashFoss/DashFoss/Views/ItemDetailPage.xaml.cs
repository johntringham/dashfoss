using DashFoss.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace DashFoss.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
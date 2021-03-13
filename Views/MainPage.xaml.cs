using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TXC54G_HF.Models;
using TXC54G_HF.Services;
using TXC54G_HF.Services.HelperModels;
using TXC54G_HF.ViewModels.Utilities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TXC54G_HF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Mode mode = Mode.Book;
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            QueryState.Text = "Query Started";
            await ViewModel.Search(Search.Text, mode);
            QueryState.Text = "Query Completed";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DetailsPage), Search.Text);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = (BaseHelper)e.ClickedItem;
            this.Frame.Navigate(typeof(DetailsPage), clicked.url);
        }

        private async void BookButton_Click_2(object sender, RoutedEventArgs e)
        {
            mode = Mode.Book;
            ContentText.Text = "Currently browsing: Books";
            QueryState.Text = "Query Started";
            await ViewModel.ListPreviews(mode);
            QueryState.Text = "Query Completed";
        }

        private async void HouseButton_Click_3(object sender, RoutedEventArgs e)
        {
            mode = Mode.House;
            ContentText.Text = "Currently browsing: Houses";
            QueryState.Text = "Query Started";
            await ViewModel.ListPreviews(mode);
            QueryState.Text = "Query Completed";
        }

        private async void CharacterButton_Click_4(object sender, RoutedEventArgs e)
        {
            mode = Mode.Character;
            ContentText.Text = "Currently browsing: Characters";
            QueryState.Text = "Query Started";
            await ViewModel.ListPreviews(mode);
            QueryState.Text = "Query Completed";
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ViewModel.previousPage();
            QueryState.Text = "Query Started";
            await ViewModel.ListNewPageOfPreviews();
            QueryState.Text = "Query Completed";
        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ViewModel.nextPage();
            QueryState.Text = "Query Started";
            await ViewModel.ListNewPageOfPreviews();
            QueryState.Text = "Query Completed";
        }

        private async void ListAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            QueryState.Text = "Query Started";
            await ViewModel.ListPreviews(mode);
            QueryState.Text = "Query Completed";
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                this.Frame.Navigate(typeof(SettingsPage));
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.ListPreviews(mode);
        }

        private async void FileButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveToFile(mode);
        }

        private async void Search_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                QueryState.Text = "Query Started";
                await ViewModel.Search(Search.Text, mode);
                QueryState.Text = "Query Completed";
            }
        }
    }
}

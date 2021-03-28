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
    /// Main page of the application, which shows a listing of character/house/book names.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// The current type of entity shown
        /// </summary>
        private Mode mode = Mode.Book;

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event handler of the search button, shows on the UI that a Query has started, then sends the search text to the ViewModel for the search.
        /// </summary>
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            QueryState.Text = "Query Started";
            await ViewModel.Search(Search.Text, mode);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// On the pressing of the enter key while modifying the search box,
        /// shows on the UI that a Query has started, then sends the search text to the ViewModel for the search.
        /// </summary>
        private async void Search_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                QueryState.Text = "Query Started";
                await ViewModel.Search(Search.Text, mode);
                QueryState.Text = "Query Completed";
            }
        }

        /// <summary>
        /// Navigates to the details page, telling it, which entity's details it should show.
        /// </summary>
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = (BaseHelper)e.ClickedItem;
            this.Frame.Navigate(typeof(DetailsPage), clicked.url);
        }

        /// <summary>
        /// Lists books and notifies the user of it.
        /// </summary>
        private async void BookButton_Click_2(object sender, RoutedEventArgs e)
        {
            mode = Mode.Book;
            ContentText.Text = "Currently browsing: Books";
            QueryState.Text = "Query Started";
            await ViewModel.ListPreviews(mode);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// Lists houses and notifies the user of it.
        /// </summary>
        private async void HouseButton_Click_3(object sender, RoutedEventArgs e)
        {
            mode = Mode.House;
            ContentText.Text = "Currently browsing: Houses";
            QueryState.Text = "Query Started";
            await ViewModel.ListPreviews(mode);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// Lists characters and notifies the user of it.
        /// </summary>
        private async void CharacterButton_Click_4(object sender, RoutedEventArgs e)
        {
            mode = Mode.Character;
            ContentText.Text = "Currently browsing: Characters";
            QueryState.Text = "Query Started";
            await ViewModel.ListPreviews(mode);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// Tells the ViewModel to show the previous page of the listing.
        /// </summary>
        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //ViewModel.previousPage();
            QueryState.Text = "Query Started";
            await ViewModel.ListPrevPageOfPreviews();
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// Tells the ViewModel to show the next page of the listing.
        /// </summary>
        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //ViewModel.nextPage();
            QueryState.Text = "Query Started";
            await ViewModel.ListNextPageOfPreviews();
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// Lists names of the current mode's entities.
        /// </summary>
        private async void ListAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            QueryState.Text = "Query Started";
            await ViewModel.ListPreviews(mode);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// On click of the settings button, navigates to the settings page.
        /// </summary>
        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                this.Frame.Navigate(typeof(SettingsPage));
            }
        }

        /// <summary>
        /// When navigated to, lists names of the current mode's entities.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.ListPreviews(mode);
        }


        /// <summary>
        /// Tells the viewmodel to save the current listing's names to a file.
        /// </summary>
        private async void FileButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveToFile(mode);
        }

      
    }
}

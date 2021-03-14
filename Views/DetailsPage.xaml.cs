using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TXC54G_HF.ViewModels.Utilities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TXC54G_HF
{
    /// <summary>
    /// Main page of the application, which shows everything about a currently selected entity.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        /// <summary>
        /// Tracks what type of entity it is showing.
        /// </summary>
        private Mode mode = Mode.Book;


        private List<StackPanel> bookControls = new List<StackPanel>();
        private List<StackPanel> characterControls = new List<StackPanel>();
        private List<StackPanel> houseControls = new List<StackPanel>();

        /// <summary>
        /// After constructing the page, the constructor fills the Controls Lists with the StackPanels on the xaml page,
        /// which contain the details about the selected entity.
        /// </summary>
        public DetailsPage()
        {
            this.InitializeComponent();
            bookControls.Add(Book1);
            bookControls.Add(Book2);
            bookControls.Add(Book3);
            characterControls.Add(Character1);
            characterControls.Add(Character2);
            characterControls.Add(Character3);
            houseControls.Add(House1);
            houseControls.Add(House2);
            houseControls.Add(House3);
        }

        /// <summary>
        /// Event handler, which lets the user go back to the previous xaml page.
        /// </summary>
        private void NavBarSide_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            _ = App.TryGoBack();
        }

        /// <summary>
        /// Event handler, which makes the UI display a character/house/book entity, depending on the NavigationEventArgs parameter.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter.ToString().Contains("characters"))
            {
                CharacterMode();
            }
            else if (e.Parameter.ToString().Contains("houses"))
            {
                HouseMode();
            }
            else if (e.Parameter.ToString().Contains("books"))
            {
                BookMode();
            }
            else
            {
                throw new Exception("Se nem karakter, se nem ház, se nem könyv? Akkor mi?");
            }
            QueryState.Text = "Query Started";
            await ViewModel.ShowDetails(e.Parameter.ToString());
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
        /// When the user clicks on a Book entity to watch its details, the UI controls get reordered to be able to show it.
        /// After the reordering, the UI shows the details of the book which fired this event.
        /// </summary>
        private async void OnBookClick(object sender, TappedRoutedEventArgs e)
        {
            BookMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            QueryState.Text = "Query Started";
            await ViewModel.ShowDetails(toShow);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// When the user clicks on a Character entity to watch its details, the UI controls get reordered to be able to show it.
        /// After the reordering, the UI shows the details of the character which fired this event.
        /// </summary>
        private async void OnCharacterClick(object sender, TappedRoutedEventArgs e)
        {
            CharacterMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            QueryState.Text = "Query Started";
            await ViewModel.ShowDetails(toShow);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// When the user clicks on a House entity to watch its details, the UI controls get reordered to be able to show it.
        /// After the reordering, the UI shows the details of the house which fired this event.
        /// </summary>
        private async void OnHouseClick(object sender, TappedRoutedEventArgs e)
        {
            HouseMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            QueryState.Text = "Query Started";
            await ViewModel.ShowDetails(toShow);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// This is where the reordering happens. The BookControls become visible, while the other 2 become hidden.
        /// </summary>
        private void BookMode()
        {
            NextButton.IsEnabled = true;
            PrevButton.IsEnabled = true;
            ContentText.Text = "Book details";
            mode = Mode.Book;
            foreach (var control in bookControls)
            {
                control.Visibility = Visibility.Visible;
            }
            foreach (var control in houseControls)
            {
                control.Visibility = Visibility.Collapsed;
            }
            foreach (var control in characterControls)
            {
                control.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// This is where the reordering happens. The HouseControls become visible, while the other 2 become hidden.
        /// </summary>
        private void HouseMode()
        {
            NextButton.IsEnabled = true;
            PrevButton.IsEnabled = true;
            ContentText.Text = "House details";
            mode = Mode.House;
            foreach (var control in bookControls)
            {
                control.Visibility = Visibility.Collapsed;
            }
            foreach (var control in houseControls)
            {
                control.Visibility = Visibility.Visible;
            }
            foreach (var control in characterControls)
            {
                control.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// This is where the reordering happens. The CharacterControls become visible, while the other 2 become hidden.
        /// </summary>
        private void CharacterMode()
        {
            NextButton.IsEnabled = false;
            PrevButton.IsEnabled = false;
            ContentText.Text = "Character details";
            mode = Mode.Character;
            foreach (var control in bookControls)
            {
                control.Visibility = Visibility.Collapsed;
            }
            foreach (var control in houseControls)
            {
                control.Visibility = Visibility.Collapsed;
            }
            foreach (var control in characterControls)
            {
                control.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Tells the ViewModel to show the next page of a list, which supports paging.
        /// These lists can be the swornMembers of a House or the Characters of a Book
        /// </summary>
        private async void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            QueryState.Text = "Query Started";
            await ViewModel.NextPage(mode);
            QueryState.Text = "Query Completed";
        }

        /// <summary>
        /// Tells the ViewModel to show the previous page of a list, which supports paging.
        /// These lists can be the swornMembers of a House or the Characters of a Book
        /// </summary>
        private async void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            QueryState.Text = "Query Started";
            await ViewModel.PreviousPage(mode);
            QueryState.Text = "Query Completed";
        }
    }
}

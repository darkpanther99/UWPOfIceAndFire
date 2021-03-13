using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        private int mode = 0;
        private List<StackPanel> bookControls = new List<StackPanel>();
        private List<StackPanel> characterControls = new List<StackPanel>();
        private List<StackPanel> houseControls = new List<StackPanel>();
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

        private void NavBarSide_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            _ = App.TryGoBack();
        }

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

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                this.Frame.Navigate(typeof(SettingsPage));
            }
        }

        private async void OnBookClick(object sender, TappedRoutedEventArgs e)
        {
            BookMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            QueryState.Text = "Query Started";
            await ViewModel.ShowDetails(toShow);
            QueryState.Text = "Query Completed";
        }

        private async void OnCharacterClick(object sender, TappedRoutedEventArgs e)
        {
            CharacterMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            QueryState.Text = "Query Started";
            await ViewModel.ShowDetails(toShow);
            QueryState.Text = "Query Completed";
        }

        private async void OnHouseClick(object sender, TappedRoutedEventArgs e)
        {
            HouseMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            QueryState.Text = "Query Started";
            await ViewModel.ShowDetails(toShow);
            QueryState.Text = "Query Completed";
        }

        private void BookMode()
        {
            NextButton.IsEnabled = true;
            PrevButton.IsEnabled = true;
            ContentText.Text = "Book details";
            mode = 0;
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

        private void HouseMode()
        {
            NextButton.IsEnabled = true;
            PrevButton.IsEnabled = true;
            ContentText.Text = "House details";
            mode = 1;
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

        private void CharacterMode()
        {
            NextButton.IsEnabled = false;
            PrevButton.IsEnabled = false;
            ContentText.Text = "Character details";
            mode = 2;
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

        private async void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            QueryState.Text = "Query Started";
            await ViewModel.NextPage(mode);
            QueryState.Text = "Query Completed";
        }

        private async void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            QueryState.Text = "Query Started";
            await ViewModel.PreviousPage(mode);
            QueryState.Text = "Query Completed";
        }
    }
}

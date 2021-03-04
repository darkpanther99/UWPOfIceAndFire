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
        }

        private void NavBarSide_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            _ = App.TryGoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
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
            ViewModel.ShowDetails(e.Parameter.ToString());
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string value = localSettings.Values["favouritehouse"] as string;
            Image img = sender as Image;
            try
            {
                if (value == null)
                {
                    img.Source = new BitmapImage(new Uri("ms-appx:///Assets/starklogo.png"));
                }
                else
                {
                    img.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{value}logo.png"));
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

        }
        private async void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                this.Frame.Navigate(typeof(SettingsPage));
            }
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.ShowDetails(ViewModel.character.father.url);
        }

        private async void OnBookClick(object sender, TappedRoutedEventArgs e)
        {
            BookMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            ViewModel.ShowDetails(toShow);
        }

        private async void OnCharacterClick(object sender, TappedRoutedEventArgs e)
        {
            CharacterMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            ViewModel.ShowDetails(toShow);
        }

        private async void OnHouseClick(object sender, TappedRoutedEventArgs e)
        {
            HouseMode();
            var src = sender as TextBlock;
            var toShow = await ViewModel.GetURIStringFromName(src.Text);
            ViewModel.ShowDetails(toShow);
        }

        private void BookMode()
        {
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


    }
}

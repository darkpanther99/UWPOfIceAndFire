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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TXC54G_HF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int mode = 0;
        //private string currentlyBrowsing = "books";
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Search(Search.Text, mode);
            //ToHide.Visibility = Visibility.Collapsed;
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

        private void BookButton_Click_2(object sender, RoutedEventArgs e)
        {
            mode = 0;
            ViewModel.currentlyBrowsing = "books";
        }

        private void HouseButton_Click_3(object sender, RoutedEventArgs e)
        {
            mode = 1;
            ViewModel.currentlyBrowsing = "houses";
        }

        private void CharacterButton_Click_4(object sender, RoutedEventArgs e)
        {
            mode = 2;
            ViewModel.currentlyBrowsing = "characters";
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ViewModel.previousPage();
            ViewModel.ListNewPageOfPreviews();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ViewModel.nextPage();
            ViewModel.ListNewPageOfPreviews();
        }

        private void ListAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ListPreviews(mode);
        }
    }

    //source: https://stackoverflow.com/questions/37087518/how-do-i-concat-string-with-the-content-which-is-a-binding-property-in-button-wh
    public class PrependStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (string)parameter + " " + (string)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // implement for two-way convertion
            throw new NotImplementedException();
        }
    }
}

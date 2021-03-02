﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using muxc = Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TXC54G_HF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public ItemClass imageitem { get; set; }
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void NavBarSide_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            _ = App.TryGoBack();
        }

        private void RadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is muxc.RadioButtons rb)
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                string housename = rb.SelectedItem as string;
                localSettings.Values["favouritehouse"] = housename;
                try
                {
                    //imageitem.Image = new BitmapImage(new Uri($"ms-appx:///Assets/starklogo.png"));
                }catch(Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }
                
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string value = localSettings.Values["favouritehouse"] as string;
            Image img = sender as Image;
            if (value == null)
            {
                img.Source = new BitmapImage(new Uri("ms-appx:///Assets/starklogo.png"));
            }
            else
            {
                img.Source = new BitmapImage(new Uri($"ms-appx:///Assets/{value}logo.png"));
            }


        }

        public class ItemClass : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private void RaiseProperty(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

            private BitmapImage image;
            public BitmapImage Image
            {
                get { return image; }
                set { image = value; RaiseProperty(nameof(Image)); }
            }

            public string Name { get; set; }
        }
    }
}

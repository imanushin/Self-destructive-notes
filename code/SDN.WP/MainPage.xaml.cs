using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SDN.WP.Resources;
using SDN.WP.Storage;

namespace SDN.WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly ObservableCollection<NoteData> notes = new ObservableCollection<NoteData>(); 

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            NoteStorage.UpdateNotes();
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            var appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            appBarButton.Text = AppResources.AddNoteIconText;
            appBarButton.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);
        }

        private void appBarButton_Click(object sender, EventArgs e)
        {
            CreateNewNote();
        }

        private void CreateNewNote()
        {
            NavigationService.Navigate(new Uri("/EditNote.xaml", UriKind.Relative));
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView(GetType().Name);
        }
    }
}
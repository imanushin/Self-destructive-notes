using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SDN.WP.Resources;
using SDN.WP.Storage;

namespace SDN.WP
{
    public partial class CreateNote : PhoneApplicationPage
    {
        private NoteData currentNote = new NoteData();

        public CreateNote()
        {
            InitializeComponent();

            title.Text = AppResources.DefaultTitle;
            titleTextBlock.Text = AppResources.CreateNote;
            UpdateLiveTime();

            BuildLocalizedApplicationBar();
        }

        private void UpdateLiveTime()
        {
            keepAliveUntil.Text = AppResources.KeepAlive + " 1 " + AppResources.Hours + " " + AppResources.Until + " " + DateTime.Now.AddHours(1).ToString("t");
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            var deleteButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/delete.png", UriKind.Relative));
            deleteButton.Text = AppResources.RemoveNote;
            ApplicationBar.Buttons.Add(deleteButton);
            deleteButton.Click += deleteButton_Click;

            var saveButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/check.png", UriKind.Relative));
            saveButton.Text = AppResources.Save;
            ApplicationBar.Buttons.Add(saveButton);
            saveButton.Click += saveButton_Click;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            NoteStorage.AddOrUpdateNote(currentNote);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            NoteStorage.RemoveNote(currentNote.Identity);
        }

        private void OnLoadedEvent(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView(GetType().Name);
        }
    }
}
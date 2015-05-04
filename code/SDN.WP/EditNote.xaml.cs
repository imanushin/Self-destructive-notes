using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SDN.Shared.Business;
using SDN.WP.Resources;
using SDN.WP.Storage;

namespace SDN.WP
{
    public partial class CreateNote : PhoneApplicationPage
    {
        [CanBeNull]
        private NoteData currentNote;

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
            if (ReferenceEquals(currentNote, null))
            {
                return;
            }

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var newNote = e.Content as NoteData;

            if (e.NavigationMode != NavigationMode.New)
            {
                return;
            }

            if (ReferenceEquals(newNote, null))
            {
                newNote = NoteData.CreateNew(AppResources.DefaultTitle);
            }

            currentNote = newNote;

            var lastSnapshot = newNote.Snapshots.Last();

            noteArea.Text = lastSnapshot.Text;
            UpdateLiveTime();
            title.Text = lastSnapshot.Title;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            saveProgressBar.Visibility = Visibility.Visible;

            var saveTask = NoteStorage.AddOrUpdateNoteAsync(currentNote);

            await saveTask.Suppress(AppResources.UnableToSaveNote);

            saveProgressBar.Visibility = Visibility.Collapsed;
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            if (ReferenceEquals(currentNote, null))
            {
                return;
            }

            await NoteStorage.RemoveNotesAsync(currentNote.Identity);
        }

        private void OnLoadedEvent(object sender, RoutedEventArgs e)
        {
        }
    }
}
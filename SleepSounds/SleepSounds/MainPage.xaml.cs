using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SleepSounds
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MenuFlyoutItem itemNew;
        String addToPlaylist; // keep track of what to add to playlist, initialise as null

        public MainPage()
        {
            this.InitializeComponent();
            initPlaylists();
            System.Diagnostics.Debug.WriteLine("App Initialised"); // testing
        }

        // Initialise/populate the playlist menu
        private async void initPlaylists()
        {
            // Read functions adapted from https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files

            // Location of folder
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;    // set folder to current working directory
            // Create a file, if one already exists open it
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists);

            var text = await FileIO.ReadLinesAsync(playlists); // read playlists file as lines
            foreach (var line in text) // loop through each line, adapted from http://stackoverflow.com/questions/22922403/not-looping-through-every-line-only-looking-at-first-line-c-sharp
            {
                string name = "" + line.Split('|')[0]; // split each line at the |, set name to string at index 0 (before the |, i.e. the name)

                // Populate MenuFlyout with item names from file
                itemNew = new MenuFlyoutItem(); // create new item on flyout
                itemNew.Name = name;            // give it a name
                itemNew.Text = name;            // actual content (what user sees)
                itemNew.Click += itemNew_Click; // create click event on item

                if (!xMenuFlyout.Items.Contains(itemNew))
                    xMenuFlyout.Items.Add(itemNew);
                else { } // do nothing

            }
        }

        // Sound buttons
        private void SoundButton_Click(object sender, RoutedEventArgs e)
        {
            Button curr = (Button)sender; // get the click event object (i.e. current sound)
            string name = curr.Name.Substring(0, curr.Name.IndexOf("_")); // get the name of the click event before the _ (birds instead of birds_Click etc)
            MediaElement me = (MediaElement)FindName(name); // set media element to be played = name

            // If / Else for stop/start on buttons
            if (me.Tag.ToString() == "N")
            {
                // if the tag is set to N (i.e. not playing), play sound and set to Y (playing)
                me.Play();
                System.Diagnostics.Debug.WriteLine("Playing: " + me.Name); // testing
                me.Tag = "Y";

                //if(addToPlaylist == null) 
                //{
                    addToPlaylist += (me.Name + ",");
                    System.Diagnostics.Debug.WriteLine("add: " + addToPlaylist);
                //}

                //else
                //{
                //    addToPlaylist += "," + me.Name;
                //    System.Diagnostics.Debug.WriteLine("add: " + addToPlaylist);
                //}
            }
            else
            {
                // Tag is set to Y (i.e. is playing), stop the sound and set tag to N, remove it from addToPlaylist string
                addToPlaylist = addToPlaylist.Replace(me.Name, null); // replace current element with an empty string
                me.Stop();
                me.Tag = "N";
                System.Diagnostics.Debug.WriteLine(me.Name + " stopped"); // testing
                System.Diagnostics.Debug.WriteLine("add: " + addToPlaylist); // testing

            }

        } // end sound buttons

        // Create a playlist
        private async void createCombo_Click(object sender, RoutedEventArgs e)
        {
            // Read/Write functions adapted from https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files

            // Create a folder and file if it doesn't exist
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;    // set folder to current working directory
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists); // Create file playlists.txt or open if exists

            // Insert name of playlist at start of playing string
            String comboName = this.inputText.Text.ToString();
            String thisPlaylist = comboName + "|" + addToPlaylist;

            // Write data to the file
            await FileIO.AppendTextAsync(playlists, thisPlaylist + Environment.NewLine); // Environment.NewLine sets pointer to new line for next entry

            System.Diagnostics.Debug.WriteLine(await FileIO.ReadTextAsync(playlists)); // testing
           
            itemNew = new MenuFlyoutItem();
            itemNew.Name = comboName;           // MenuFlyoutItem name & displayed name are the same
            itemNew.Text = comboName;
            itemNew.Click += itemNew_Click;     // Create click method/event for new item
            xMenuFlyout.Items.Add(itemNew);     // add itemNew to MenuFlyout

        }

        private async void itemNew_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem curr = (MenuFlyoutItem)sender; // let current item equal to data sent by MenuFlyoutItem
            
            // Open and read contents of file
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists);

            var text = await FileIO.ReadLinesAsync(playlists); // read playlists file as lines

            // Search through text, find line beginning with name,
            // split line into songs string, for each song in songs add it to the songsArray
            // search through songsArray, find matching ME for each element of the array & play it
            // probably shorter way to do it but works for now
            // stop button for all songs?
            
            String[] songsArray;

            foreach (var line in text)
            {
                if (line.StartsWith(curr.Text) == true)
                {
                    string name = line.Split('|')[0]; // split each line at the |, set name to string at index 0 (before the |, i.e. the name)
                    string songs = line.Split('|')[1]; // after the | = songs list
                    System.Diagnostics.Debug.WriteLine("name: " + name + ", " + curr.Text); // testing

                    while (songs != null)
                    {
                        songsArray = songs.Split(','); // Split songs into songsArray, adapted from https://msdn.microsoft.com/en-us/library/ms228388.aspx
                        foreach (var song in songsArray)   // loop through songsArray
                        {
                            System.Diagnostics.Debug.WriteLine(song); // testing
                            if (song != "") 
                            {
                                // fixes bug with removing stopped songs from addToPlaylist string, e.g. string might be ",,rain,thunder,"
                                // null elemement taken into account but not played, quickest workaround for now
                                MediaElement me = (MediaElement)FindName(song.ToString());
                                me.Play();
                            }
                            else { } // do nothing
                            
                        } // end foreach

                    } // end while for searching line

                } // end if right playlist

                else { } // not the line/playlist you're looking for so do nothing

            } // end foreach line in text

        } // end item click

        private void flyout_btn_Click(object sender, RoutedEventArgs e)
        {
            // just open the menu, don't do anything
            System.Diagnostics.Debug.WriteLine("flyout clicked"); // testing
        }

    }// end mainpage
} // end app


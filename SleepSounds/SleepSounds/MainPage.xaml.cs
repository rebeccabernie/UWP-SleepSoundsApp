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

        //List<string> _listNames;
        //List<string> _playListsFromFile;
        //List<string> _currentPlaylistSongs;


        String[] songsList = new String[]
           { "birds", "cat", "city", "fire", "forrain", "rain", "thunder", "waves,", "whitenoise" };
        String playing; // keep track of currently playing, set to empty
        String addToPlaylist; // what to add to playlist

        public MainPage()
        {
            this.InitializeComponent();
            initPlaylists();
            System.Diagnostics.Debug.WriteLine("App Initialised"); // testing
        }

        // Initialise/populate the playlist menu
        private async void initPlaylists()
        {
            // Location of folder
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;    // set folder to current working directory
            // Create a file, if one already exists open it
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists);

            var text = await FileIO.ReadLinesAsync(playlists); // read playlists file as lines
            foreach (var line in text) // loop through each line, adapted from http://stackoverflow.com/questions/22922403/not-looping-through-every-line-only-looking-at-first-line-c-sharp
            {
                string name = "" + line.Split('|')[0]; // split each line at the |, set name to string at index 0 (before the |, i.e. the name)

                // Populate MenuFlyout with items from file
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
            string name = curr.Name.Substring(0, curr.Name.IndexOf("_")); // get the name of the click event before the _
            MediaElement me = (MediaElement)FindName(name); // set media element = new name

            // If / Else for stop/start on buttons
            if (me.Tag.ToString() == "N")
            {
                // if the tag is set to N (i.e. not playing), play sound and set to Y (playing)
                me.Play();
                System.Diagnostics.Debug.WriteLine("Playing: " + me.Name); // testing
                me.Tag = "Y";
                if(playing == null)
                {
                    playing += me.Name; // Add to current playing list
                    addToPlaylist += me.Name; // Add to list of songs for playlist
                }
                else
                {
                    playing += ("," + me.Name);
                    addToPlaylist += ("," + me.Name);
                }
            }
            else
            {
                // Tag is set to Y (i.e. is playing), stop the sound and set tag to N
                me.Stop();
                System.Diagnostics.Debug.WriteLine(me.Name + " stopped"); // testing
                me.Tag = "N";
                playing.Replace(me.Name, null);   // Remove from currently playing list / replace with nothing
                //addToPlaylist.Replace(me.Name, ""); // Remove from list of songs for playlist

            }

        } // end sound buttons

        // Create a playlist
        private async void createCombo_Click(object sender, RoutedEventArgs e)
        {
            // Read/Write functions adapted from https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files

            // Create a folder and file if it doesn't exist
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;    // set folder to current working directory
            StorageFile playlists =
                await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists); // Make new

            // Insert name of playlist at start of playing string
            String comboName = this.inputText.Text.ToString();
            String playlist = comboName + "|" + addToPlaylist;

            // Write data to the file
            await FileIO.AppendTextAsync(playlists, playlist + Environment.NewLine); // Environment.NewLine sets pointer to new line for next entry
            // Read data from file
            //await FileIO.ReadTextAsync(playlists);

            System.Diagnostics.Debug.WriteLine(await FileIO.ReadTextAsync(playlists)); // test, print to the debug console

            addToPlaylist = null; // reset the playing string when user saves a combo

           
            itemNew = new MenuFlyoutItem();
            itemNew.Name = comboName;
            itemNew.Text = comboName;
            itemNew.Click += itemNew_Click;
            
            //if (!xMenuFlyout.Items.Contains(itemNew)) // if the menu doesn't contain the
            //    xMenuFlyout.Items.Add(itemNew);
            //else { } // do nothing

        }

        private async void itemNew_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem curr = (MenuFlyoutItem)sender; // let current item equal to data sent by button
            // Open and read contents of file
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists);

            var text = await FileIO.ReadLinesAsync(playlists); // read playlists file as lines

            // Search through _playlistsFromFile array, find line beginning with name,
            // split line and parse songs into array, keep playing
            // stop button for all songs?

            //List<string> songNames = new List<string>(); // list of song names

            List<string> _listNames = new List<string>();
            List<string> _currentPlaylistSongs = new List<string>();
            String[] songsArray;

            foreach (var line in text)
            {
                if (line.StartsWith(curr.Text) == true)
                {
                    string name = line.Split('|')[0]; // split each line at the |, set name to string at index 0 (before the |, i.e. the name)
                    //_listNames.Add(name.ToString()); // add name to the array list of names
                    string songs = line.Split('|')[1];
                    System.Diagnostics.Debug.WriteLine("name: " + name + ", " + curr.Text);

                    //string[] split = songs.Split(',');

                    //int start = 1;
                    while (songs != null)
                    {
                        //string song = songs.Substring(start, songs.IndexOf(",") - start);
                        //https://msdn.microsoft.com/en-us/library/ms228388.aspx
                        songsArray = songs.Split(',');
                        foreach (var song in songsArray)
                        {
                            System.Diagnostics.Debug.WriteLine(song);
                            //_currentPlaylistSongs.Add(song);

                            MediaElement me = (MediaElement)FindName(song.ToString());
                            me.Play();
                        }
                        
                        //start = songs.IndexOf(",") + 1;

                    }

                }
                else { }

            }
        }

        private async void flyout_btn_Click(object sender, RoutedEventArgs e)
        {
            // just open the menu, don't do anything
            System.Diagnostics.Debug.WriteLine("flyout clicked");
        }

    }// end mainpage
} // end app


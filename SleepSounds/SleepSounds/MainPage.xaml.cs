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
        MenuFlyoutSubItem itemNew;
        MenuFlyoutItem delete;
        MenuFlyoutItem play;


        //String addToPlaylist; // keep track of what to add to playlist, initialise as null
        String[] songsList = { "birds", "cat", "city", "fire", "forrain", "rain", "thunder", "waves", "whitenoise" };

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
                itemNew = new MenuFlyoutSubItem(); // create new item on flyout
                itemNew.Name = name;            // give it a name
                itemNew.Text = name;            // actual content (what user sees)
                //itemNew.Click += itemNew_Click; // create click event on item

                playPL = new MenuFlyoutItem();
                playPL.Click += itemNew_Click;
                deletePlaylist = new MenuFlyoutItem();
                deletePlaylist.Click += delete_Click;


                if (!xMenuFlyout.Items.Contains(itemNew))
                {
                    xMenuFlyout.Items.Add(itemNew);
                    xMenuFlyout.Items.Add(playPL);
                    xMenuFlyout.Items.Add(deletePlaylist);

                }

                else { } // do nothing

            }
        }

        private void playSound(string name)
        {
            MediaElement me = (MediaElement)FindName(name); // set media element to be played = name

            // If / Else for stop/start on buttons
            if (me.Tag.ToString() == "N")
            {
                // if the tag is set to N (i.e. not playing), play sound and set to Y (playing)
                me.Play();
                System.Diagnostics.Debug.WriteLine("Playing: " + me.Name); // testing
                me.Tag = "Y";

                Button curr = (Button)FindName(me.Name + "_Btn");
                curr.Content = "♫ " + curr.Content; // found ♫ symbol on http://usefulshortcuts.com/alt-codes/bullet-alt-codes.php
            }
            else
            {
                // Tag is set to Y (i.e. is playing), stop the sound and set tag to N, remove it from addToPlaylist string
                //addToPlaylist = addToPlaylist.Replace(me.Name, null); // replace current element with an empty string
                me.Stop();
                me.Tag = "N";
                Button curr = (Button)FindName(me.Name + "_Btn");
                curr.Content = curr.Content.ToString().Replace("♫ ", "");

                System.Diagnostics.Debug.WriteLine(me.Name + " stopped"); // testing
                //System.Diagnostics.Debug.WriteLine("add: " + addToPlaylist); // testing
            }

        }

        private void playPlaylist(string songs)
        {
            String[] songsArray = songs.Split(',');
            System.Diagnostics.Debug.WriteLine(songsArray);

            foreach (string song in songsArray)
            {
                if (song == "") { }
                else
                {
                    MediaElement me = (MediaElement)FindName(song); // set media element to be played = name
                    System.Diagnostics.Debug.WriteLine(song);
                    Button curr = (Button)FindName(me.Name + "_Btn");
                    if (me.Tag.ToString() == "Y") { 
                        curr.Content = curr.Content.ToString().Replace("♫ ", "");
                        me.Tag = "N";
                        me.Stop();
                    }
                    else { 
                        curr.Content = "♫ " + curr.Content;
                        me.Tag = "Y";
                        me.Play();
                    }
                    
                    //me.Tag = "Y"; // set tag to playing
                }
                    
            }

        }

        // Sound buttons
        private void SoundButton_Click(object sender, RoutedEventArgs e)
        {
            Button curr = (Button)sender; // get the click event object (i.e. current sound)
            string name = curr.Name.Substring(0, curr.Name.IndexOf("_")); // get the name of the click event before the _ (birds instead of birds_Click etc)

            playSound(name);

        } // end sound buttons

        // Create a playlist
        private async void createCombo_Click(object sender, RoutedEventArgs e)
        {
            // Read/Write functions adapted from https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files

            // Create a folder and file if it doesn't exist
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;    // set folder to current working directory
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists); // Create file playlists.txt or open if exists
            var text = await FileIO.ReadLinesAsync(playlists);
            // Insert name of playlist at start of playing string
            String comboName = this.inputText.Text.ToString();
            bool alreadyExists = false;

            foreach (var line in text)
            {
                if (line.StartsWith(comboName))
                {
                    this.popup.IsOpen = true;
                    alreadyExists = true;
                    
                }
                else { }
            }

            if (alreadyExists != true)
            {
                String addToPlaylist = "";

                foreach (string song in songsList)
                {
                    MediaElement me = (MediaElement)FindName(song); // set media element to be played = name
                    if (me.Tag.ToString() == "Y")
                    {
                        addToPlaylist += "," + song;
                    }
                    else { }
                }
                String thisPlaylist = comboName + "|" + addToPlaylist;

                // Write data to the file
                await FileIO.AppendTextAsync(playlists, thisPlaylist + Environment.NewLine); // Environment.NewLine sets pointer to new line for next entry
                System.Diagnostics.Debug.WriteLine(await FileIO.ReadTextAsync(playlists)); // testing

                itemNew = new MenuFlyoutSubItem();
                itemNew.Name = comboName;           // MenuFlyoutItem name & displayed name are the same
                itemNew.Text = comboName;
                //itemNew.Click += itemNew_Click;     // Create click method/event for new item
                playPL = new MenuFlyoutItem();
                playPL.Click += itemNew_Click;
                deletePlaylist = new MenuFlyoutItem();
                deletePlaylist.Click += delete_Click;
                

                xMenuFlyout.Items.Add(itemNew);     // add itemNew to MenuFlyout
            }
           
        }// end combo

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            MenuFlyoutItem mi = (MenuFlyoutItem)sender;
            string name = mi.Name;
            xMenuFlyout.Items.Remove(mi);

            // Create a folder and file if it doesn't exist
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;    // set folder to current working directory
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists); // Create file playlists.txt or open if exists
            var text = await FileIO.ReadLinesAsync(playlists);

            await FileIO.WriteTextAsync(playlists, ""); // clear contents of file
            System.Diagnostics.Debug.WriteLine("Empty: " + playlists);

            foreach (var line in text)
            {
                if (!line.StartsWith(name)) // if line doesn't start with item for deletion, add to new file
                {
                    await FileIO.WriteTextAsync(playlists, line + Environment.NewLine); // Environment.NewLine sets pointer to new line for next entry
                }
                else { } // is the file for deletion so don't add to new file
            }


        }

        private async void itemNew_Click(object sender, RoutedEventArgs e)
        {
            // Open and read contents of file
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists);
            var text = await FileIO.ReadLinesAsync(playlists); // read playlists file as lines

            MenuFlyoutItem curr = (MenuFlyoutItem)sender; // let current item equal to data sent by MenuFlyoutItem

            foreach (var line in text)
            {
                if (line.StartsWith(curr.Text) == true)
                {
                    string name = line.Split('|')[0]; // split each line at the |, set name to string at index 0 (before the |, i.e. the name)
                    string songs = line.Split('|')[1]; // after the | = songs list
                    System.Diagnostics.Debug.WriteLine("name: " + name + ", " + curr.Text); // testing

                    System.Diagnostics.Debug.WriteLine(songs);
                    playPlaylist(songs);

                }
                else { }
            }
        }// end item click

        private void flyout_btn_Click(object sender, RoutedEventArgs e)
        {
            // just open the menu, don't do anything
            System.Diagnostics.Debug.WriteLine("flyout clicked"); // testing
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in songsList)
            {
                MediaElement me = (MediaElement)FindName(item); // set media element to be played = name

                // If / Else for stop/start on buttons
                if (me.Tag.ToString() == "Y")
                {
                    me.Stop();
                    me.Tag = "N";
                    Button curr = (Button)FindName(me.Name + "_Btn");
                    curr.Content = curr.Content.ToString().Replace("♫ ", "");
                }
                else{ }


            }

        }

        private void popup_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.popup.IsOpen = false;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }
    } // end mainpage

} // end app


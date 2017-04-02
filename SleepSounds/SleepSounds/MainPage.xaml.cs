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
        // Settings
        Windows.Storage.ApplicationDataContainer localSettings =
            Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;

        String[] songs = new String[] 
            { "birds", "cat", "city", "fire", "forrain", "rain", "thunder", "waves,", "whitenoise" };
        String playing; // currently playing, set to empty

        public MainPage()
        {
            this.InitializeComponent();
        }

        // Sound buttons
        private void SoundButton_Click(object sender, RoutedEventArgs e)
        {
            Button curr = (Button)sender; // get the click event object (i.e. current sound)
            string name = curr.Name.Substring(0, curr.Name.IndexOf("_")); // get the name of the click event before the _
            MediaElement me = (MediaElement)FindName(name); // set media element = new name

            // If / Else for stop/start on buttons
            if(me.Tag.ToString() == "N"){ 
                // if the tag is set to N (i.e. not playing), play sound and set to Y (playing)
                me.Play();
                me.Tag = "Y";
                playing += ("," + me.Name); // Add to playing list
            }
            else{
                // Tag is set to Y (i.e. is playing), stop the sound and set tag to N
                me.Stop();
                me.Tag = "N";
                playing.Replace(me.Name, "");   // Remove from playing list / replace with nothing

            }

        } // end sound buttons

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
            String playlist = comboName + "|" + playing;

            // Write data to the file
            await FileIO.AppendTextAsync(playlists, playlist + Environment.NewLine); // Environment.NewLine sets pointer to new line for next entry
            // Read data from file
            //await FileIO.ReadTextAsync(playlists);

            System.Diagnostics.Debug.WriteLine(await FileIO.ReadTextAsync(playlists)); // test, print to the debug console

            playing = ""; // reset the playing string when user saves a combo
        }

        private void stopAll_Click(object sender, RoutedEventArgs e)
        {
            playing = ""; // reset playing string
        }

        private async void flyout_btn_Click(object sender, RoutedEventArgs e)
        {
            // Open and read contents of file
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists);
            var text = await FileIO.ReadLinesAsync(playlists); // read playlists file as lines

            foreach (var line in text) // loop through each line, adapted from http://stackoverflow.com/questions/22922403/not-looping-through-every-line-only-looking-at-first-line-c-sharp
            {
                string name = "" + line.Split('|')[0]; // split each line at the |, set name to string at index 0 (before the |, i.e. the name)
                System.Diagnostics.Debug.WriteLine("name: "+ name); // testing

                item.Text = name;
            }

        }

        private async void item_Click(object sender, RoutedEventArgs e)
        {
            // Open and read contents of file
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile playlists = await storageFolder.CreateFileAsync("playlists.txt", CreationCollisionOption.OpenIfExists);

            var text = await FileIO.ReadLinesAsync(playlists); // read playlists file as lines
            
            foreach (var line in text)
            {
                int i = 0;
                while (i == 0){
                    if (line.StartsWith(item.Name))
                    {
                        birds.Play();
                        i = 1;
                    }
                     
                    else{
                        city.Play();
                    }

                }
                
                    
            }
        }
    } // end mainpage

} // end app

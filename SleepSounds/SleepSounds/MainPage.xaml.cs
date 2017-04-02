using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        String[] playing = new String[]{ }; // currently playing

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

            me.Play();
        } // end sound buttons

        private void createCombo_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["exampleSetting"] = "Hello Windows";
            Object value = localSettings.Values["exampleSetting"];
            
            foreach (String i in songs)
            {
                //if ()
                    // song is playing, add to playing array

            }

            /*
            StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt",
                CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, value.ToString);*/
        }
    } // end mainpage

} // end app

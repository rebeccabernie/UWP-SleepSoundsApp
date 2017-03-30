using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SleepSounds
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private String[] playing = new String[5];
        public MainPage()
        {
            this.InitializeComponent();
        }

        // Arrays
        void PlayMultiple()//this should loop through an array of names, set the media element name and play
        {
            MediaElement me = new MediaElement();
           
            for(int i = 0; i < playing.Length; i++)
            {

                //playing stores the media elements names
                me.Name = playing[i];
                me.Play();
            }
        }

        // Count Variables
        public int rainC = 0;
        public int waveC = 0;
        public int thunC = 0;
        public int wNoiseC = 0;
        public int fireC = 0;
        public int forThuC = 0;
        public int cityC = 0;
        public int birdsC = 0;

        //isPlaying Variables
        public bool rainIP = false;
        public bool waveIP = false;
        public bool thunIP = false;
        public bool whIP = false;
        public bool fireIP = false;
        public bool forthunIP = false;
        public bool cityIP = false;
        public bool birdsIP = false;


    // Button methods
    private void rainButton_Click(object sender, RoutedEventArgs e)
        {
            if (rainC == 0)
            {
                medrain.Play();
                rainC++;
                rainIP = true;
                
            }
            else
            {
                medrain.Stop();
                rainC--;
                rainIP = false;
            }
        }

        private void wavButton_Click(object sender, RoutedEventArgs e)
        {
            if (waveC == 0)
            {
                waves.Play();
                waveC++;
                waveIP = true;

            }
            else
            {
                waves.Stop();
                waveC--;
                waveIP = false;
            }
        }

        private void thuButton_Click(object sender, RoutedEventArgs e)
        {
            if (thunC == 0)
            {
                thunder.Play();
                thunC++;
                thunIP = true;
            }
            else
            {
                thunder.Stop();
                thunC--;
                thunIP = false;
            }
        }

        private void whnoiButton_Click(object sender, RoutedEventArgs e)
        {
            if (wNoiseC == 0)
            {
                whitenoise.Play();
                wNoiseC++;
                whIP = true;
            }
            else
            {
                whitenoise.Stop();
                wNoiseC--;
                whIP = false;
            }
        }

        private void fireButton_Click(object sender, RoutedEventArgs e)
        {
            if (fireC == 0)
            {
                fire.Play();
                fireC++;
                fireIP = true;
            }
            else
            {
                fire.Stop();
                fireC--;
                fireIP = false;
            }
        }

        private void cityButton_Click(object sender, RoutedEventArgs e)
        {
            if (cityC == 0)
            {
                city.Play();
                cityC++;
                cityIP = true;
            }
            else
            {
                city.Stop();
                cityC--;
                cityIP = false;
            }
        }

        private void birdsButton_Click(object sender, RoutedEventArgs e)
        {
            if (birdsC == 0)
            {
                birds.Play();
                birdsC++;
                birdsIP = true;
            }
            else
            {
                birds.Stop();
                birdsC--;
                birdsIP = false;
            }
        }

        private void createCombo_Click(object sender, RoutedEventArgs e)
        {
            MediaElement me = new MediaElement();
            //playing[0] = ;
        }

        private void SoundButton_Click(object sender, RoutedEventArgs e)
        {
            Button curr = (Button)sender;
            // get substring
            string name = curr.Name.Substring(0, curr.Name.IndexOf("_"));
            MediaElement me = (MediaElement)FindName(name);

            if (me.Tag.ToString() == "N")
            {
                me.Play();
                me.Tag = "Y";
            }
            else
            {
                me.Stop();
                me.Tag = "N";
            }

        }

        private void forRainButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

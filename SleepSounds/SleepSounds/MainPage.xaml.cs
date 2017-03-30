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
        public MainPage()
        {
            this.InitializeComponent();
        }

        // Count Variables
        public int rainC = 0;
        public int waveC = 0;
        public int thunC = 0;
        public int wNoiseC = 0;
        public int fireC = 0;
        public int forThuC = 0;
        public int forRainC = 0;
        public int cityC = 0;
        public int birdsC = 0;


        private void rainButton_Click(object sender, RoutedEventArgs e)
        {
            if (rainC == 0)
            {
                medrain.Play();
                rainC++;
            }
            else
            {
                medrain.Stop();
                rainC--;
            }
        }

        private void wavButton_Click(object sender, RoutedEventArgs e)
        {
            if (waveC == 0)
            {
                
               // waves.PlayLooping();
                waveC++;
            }
            else
            {
                waves.Stop();
                waveC--;
            }
        }

        private void thuButton_Click(object sender, RoutedEventArgs e)
        {
            if (thunC == 0)
            {
                thunder.Play();
                thunC++;
            }
            else
            {
                thunder.Stop();
                thunC--;
            }
        }

        private void whnoiButton_Click(object sender, RoutedEventArgs e)
        {
            if (wNoiseC == 0)
            {
                whitenoise.Play();
                wNoiseC++;
            }
            else
            {
                whitenoise.Stop();
                wNoiseC--;
            }
        }

        private void fireButton_Click(object sender, RoutedEventArgs e)
        {
            if (fireC == 0)
            {
                fire.Play();
                fireC++;
            }
            else
            {
                fire.Stop();
                fireC--;
            }
        }

        private void forThunderButton_Click(object sender, RoutedEventArgs e)
        {
            if (forThuC == 0)
            {
                forthunder.Play();
                forThuC++;
            }
            else
            {
                forthunder.Stop();
                forThuC--;
            }

        }

        private void forRainButton_Click(object sender, RoutedEventArgs e)
        {
            if (forRainC == 0)
            {
                forrain.Play();
                forRainC++;
            }
            else
            {
                forrain.Stop();
                forRainC--;
            }

        }

        private void cityButton_Click(object sender, RoutedEventArgs e)
        {
            if (cityC == 0)
            {
                city.Play();
                cityC++;
            }
            else
            {
                city.Stop();
                cityC--;
            }
        }

        private void birdsButton_Click(object sender, RoutedEventArgs e)
        {
            if (birdsC == 0)
            {
                birds.Play();
                birdsC++;
            }
            else
            {
                birds.Stop();
                birdsC--;
            }
        }
    }
}

﻿using SkiAppClient.Common;
using SkiAppClient.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace SkiAppClient
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SlopeInformationPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SlopeInformationPage"/> class.
        /// </summary>
        public SlopeInformationPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
           
            if (e.NavigationParameter != null)
            {
                var destination = (Destination)e.NavigationParameter;
                
                if (destination != null)
                {
                    var slopeInformation = GetSlopeInformation(destination);
                    try
                    {
                        this.DefaultViewModel["SlopeInformation"] = slopeInformation;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        this.DefaultViewModel["SlopeInformation"] = null;
                        try
                        {
                            MessageDialog md = new MessageDialog("Får ikke vist løypeinformasjon. Sjekk internettkoblingen din og prøv på nytt!");
                            md.ShowAsync();
                        }
                        catch (UnauthorizedAccessException)
                        {
                            //Dette skjer dersom brukeren får beskjed fra et annet sted om at noe gikk galt. 
                            //Trenger ikke gjøre noe med exception bare catche det så ikke programmet krasjer.
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the slope information for a given destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <returns>SlopeDestination</returns>
        /// 
        // Denne metoden scorer dårlig på code metrics. 56 på Maintainability Index og 14 på Lines of code. Har forklart hvorfor jeg velger å gjøre det på denne måten i rapporten under kjente problemer.
        // Er ikke noe mer jeg kan skille ut i egne metoder.
        private static SlopeInformation GetSlopeInformation(Destination destination)
        {
            SlopeInformation slopeInformation = new SlopeInformation();
            switch (destination.DestinationName)
            {
                case "Hemsedal":
                    slopeInformation = new SlopeInformation("Hemsedal", "20 heiser", "49 bakker", "3 parker og 1 skicrossarena",
                        "Stort barneområde med barnevennlige heiser og bakker", "Arena for speedtesting, parallellslalåm, big air bag, skøytebane m.m", "Assets/Loypekart/loypekartHemsedal.PNG");
                    break;
                case "Trysil":
                    slopeInformation = new SlopeInformation("Trysil", "31 heiser", "68 bakker", "Terrengparker for alle nivåer",
                        "3 barneområder", "Arena for cross, speedtesting, parallellslalåm, self-timer, kuler m.m", "Assets/Loypekart/loypekartTrysil.PNG");
                    break;
                case "Vemdalen":
                    slopeInformation = new SlopeInformation("Vemdalen", "50 heiser", "110 bakker", "Terrengparker for alle nivåer",
                "4 barneområder", "Arena for cross, speedtesting, parallellslalåm, self-timer, kuler m.m", "Assets/Loypekart/loypekartVemdalen.PNG");
                    break;
                case "Sälen":
                    slopeInformation = new SlopeInformation("Sälen", "36 heiser", "64 bakker", "Terrengparker for alle nivåer",
                "2 barneområder", "Arena for cross, speedtesting, parallellslalåm m.m", "Assets/Loypekart/loypekartSalen.PNG");
                    break;
                case "Åre":
                    slopeInformation = new SlopeInformation("Åre", "46 heiser", "120 bakker", "Terrengparker for alle nivåer",
                "1 barneområder", "Arena for cross, speedtesting, parallellslalåm, kuler m.m", "Assets/Loypekart/loypekartAare.PNG");
                    break;
            }
            return slopeInformation;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Invoked immediately after the Page is unloaded and is no longer the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the navigation that has unloaded the current Page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the StartPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void StartPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }
    }
}

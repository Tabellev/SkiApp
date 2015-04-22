using SkiAppClient.Common;
using SkiAppClient.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class SkiDayPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ObservableCollection<Lift> lifts;
        private ObservableCollection<string> liftNames;
        private ObservableCollection<Slope> slopes;
        private ObservableCollection<string> slopeNames;
        private User user;
        private ObservableCollection<Destination> destinations; 

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


        public SkiDayPage()
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
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            lifts = new ObservableCollection<Lift>();
            liftNames = new ObservableCollection<string>();
            slopes = new ObservableCollection<Slope>();
            slopeNames = new ObservableCollection<string>();
            user = (User)e.NavigationParameter;
            destinations = await SkiAppDataSource.GetDestinationAsync();
            foreach (var destination in destinations)
            {
                cbDestinations.Items.Add(destination.DestinationName);
            }

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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void SaveSkiDay_Click(object sender, RoutedEventArgs e)
        {
            var destinationName = cbDestinations.SelectedItem.ToString();
            var date = tbDate.Text;
            var fromClock = tbFromClock.Text;
            var toClock = tbToClock.Text;
            var equipment = tbEquipment.Text;
            var totalTrips = Convert.ToInt32(tbTotalTrips.Text);
            var comment = tbComment.Text;
            ObservableCollection<Lift> destinationLifts = new ObservableCollection<Lift>();
            ObservableCollection<Slope> destinationSlopes = new ObservableCollection<Slope>();
            foreach(var lift in lifts)
            {
                foreach (var chosenLift in liftNames)
                {
                    if (lift.LiftName.Equals(chosenLift))
                    {
                        destinationLifts.Add(lift);
                    }
                }
            }

            foreach (var slope in slopes)
            {
                foreach (var chosenSlope in slopeNames)
                {
                    if (slope.SlopeName.Equals(chosenSlope))
                    {
                        destinationSlopes.Add(slope);
                    }
                }
            }

           await SkiAppDataSource.AddSkiDayAsync(user, destinationName, date, fromClock, toClock, equipment, totalTrips, comment, destinationLifts, destinationSlopes);
           MessageDialog md = new MessageDialog("Skidag lagret!");
           await md.ShowAsync();
        }

        private void SeeHistory_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HistoryPage), user);
        }

        private void AddLift_Click(object sender, RoutedEventArgs e)
        {
            var lift = cbLifts.SelectedItem.ToString();
            liftNames.Add(lift);
            cbChosenLifts.Items.Add(lift);
        }

        private void AddSlope_Click(object sender, RoutedEventArgs e)
        {
            var slope = cbSlopes.SelectedItem.ToString();
            slopeNames.Add(slope);
            cbChosenSlopes.Items.Add(slope);
        }

        private async void cbDestinations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Destination chosenDestination = null;
            //List<string> dest = new List<string>();
            this.cbLifts.Items.Clear();
            this.cbSlopes.Items.Clear();
            slopes = await SkiAppDataSource.GetSlopesAsync();
            if (cbDestinations.SelectedItem != null)
            {
                var selectedDestination = (string)cbDestinations.SelectedItem.ToString();
                 lifts = await SkiAppDataSource.GetLiftsAsync();
                
                
                foreach (var d in destinations)
                {
                    if (d.DestinationName.Equals(selectedDestination))
                    {
                        foreach (var lift in lifts)
                        {
                            var destination = (Destination)lift.LiftDestination;
                            if (destination.DestinationId.Equals(d.DestinationId))
                            {
                                cbLifts.Items.Add(lift.LiftName);
                            }
                        }

                        
                        foreach (var s in slopes)
                        {
                            var sd = (Destination)s.SlopeDestination;
                            
                            if (sd.DestinationId.Equals(d.DestinationId))
                            {
                               cbSlopes.Items.Add(s.SlopeName);
                            }
                        }
                    }
                }

                if (chosenDestination != null)
                {
                    chosenDestination = await SkiAppDataSource.GetOneDestinationAsync(chosenDestination.DestinationId);

                }


            }

        }

        private void RemoveLift_Click(object sender, RoutedEventArgs e)
        {
           cbChosenLifts.Items.Remove(cbChosenLifts.SelectedItem);
        }

        private void RemoveSlope_Click(object sender, RoutedEventArgs e)
        {
            cbChosenSlopes.Items.Remove(cbChosenSlopes.SelectedItem);
        }
    }
}

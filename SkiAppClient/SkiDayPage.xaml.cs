using SkiAppClient.Common;
using SkiAppClient.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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


        /// <summary>
        /// Initializes a new instance of the <see cref="SkiDayPage"/> class.
        /// </summary>
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
            if (e.NavigationParameter != null)
            {
                user = (User)e.NavigationParameter;
            }

            destinations = await SkiAppDataSource.GetDestinationsAsync();
            if (destinations != null && destinations.Count != 0)
            {
                foreach (var d in destinations)
                {
                    cbDestinations.Items.Add(d.DestinationName);
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("Skidag kan ikke opprettes. Sjekk internettkoblingen din og prøv på nytt!");
                await md.ShowAsync();
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

        /// <summary>
        /// Handles the Click event of the SaveSkiDay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SaveSkiDay_Click(object sender, RoutedEventArgs e)
        {
            int totalTrips;
            var destinationName = "";
            if (cbDestinations.SelectedItem != null)
            {
               destinationName = cbDestinations.SelectedItem.ToString();
            }

            var dayOfWeek = "";

            switch (dpDate.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dayOfWeek = NorwegianDayOfWeek.Mandag.ToString();
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeek = NorwegianDayOfWeek.Tirsdag.ToString();
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeek =NorwegianDayOfWeek.Onsdag.ToString();
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeek = NorwegianDayOfWeek.Torsdag.ToString();
                    break;
                case DayOfWeek.Friday:
                    dayOfWeek = NorwegianDayOfWeek.Fredag.ToString();
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeek = NorwegianDayOfWeek.Lørdag.ToString();
                    break;
                case DayOfWeek.Sunday:
                    dayOfWeek = NorwegianDayOfWeek.Søndag.ToString();
                    break;
            }
           
            var date = dayOfWeek + " " + dpDate.Date.ToString("dd/MM-yyyy");
            
            var fromClock = tpFromClock.Time.ToString().Substring(0,5);
            var toClock = tpToClock.Time.ToString().Substring(0,5);
            var equipment = tbEquipment.Text;
            if (tbTotalTrips.Text.Equals(""))
            {
                totalTrips = 0;
            }
            else
            {
                totalTrips = Convert.ToInt32(tbTotalTrips.Text);
            }
          
            var comment = tbComment.Text;
            ObservableCollection<Lift> destinationLifts = new ObservableCollection<Lift>();
            ObservableCollection<Slope> destinationSlopes = new ObservableCollection<Slope>();

            if (lifts != null && lifts.Count != 0)
            {
                foreach (var lift in lifts)
                {
                    if (liftNames != null && liftNames.Count != 0)
                    {
                        foreach (var chosenLift in liftNames)
                        {
                            if (lift.LiftName.Equals(chosenLift))
                            {
                                destinationLifts.Add(lift);
                            }
                        }
                    }
                }
            }

            if (slopes != null && slopes.Count != 0)
            {
                foreach (var slope in slopes)
                {
                    if (slopeNames != null && slopeNames.Count != 0)
                    {
                        foreach (var chosenSlope in slopeNames)
                        {
                            if (slope.SlopeName.Equals(chosenSlope))
                            {
                                destinationSlopes.Add(slope);
                            }
                        }
                    }
                }
            }

            if (user != null)
            {
                await SkiAppDataSource.AddSkiDayAsync(user, destinationName, date, fromClock, toClock, equipment, totalTrips, comment, destinationLifts, destinationSlopes);
                MessageDialog md = new MessageDialog("Skidag lagret!");
                await md.ShowAsync();
            }
            else
            {
                MessageDialog md = new MessageDialog("Skidag ikke lagret. Prøv på nytt!!");
                await md.ShowAsync();
            }

           tbEquipment.Text = "";
           tbTotalTrips.Text = "";
           tbComment.Text = "";
           cbDestinations.SelectedItem = null;
           cbChosenLifts.Items.Clear();
           cbChosenSlopes.Items.Clear();
        }

        /// <summary>
        /// Handles the Click event of the SeeHistory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SeeHistory_Click(object sender, RoutedEventArgs e)
        {
                this.Frame.Navigate(typeof(HistoryPage), user);
        }

        /// <summary>
        /// Handles the Click event of the AddLift control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddLift_Click(object sender, RoutedEventArgs e)
        {
            if (cbLifts.SelectedItem != null)
            {
                var lift = cbLifts.SelectedItem.ToString();
                liftNames.Add(lift);
                cbChosenLifts.Items.Add(lift);
                cbLifts.Items.Remove(lift);
                cbChosenLifts.SelectedIndex = 0;
            }
       }

        /// <summary>
        /// Handles the Click event of the AddSlope control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddSlope_Click(object sender, RoutedEventArgs e)
        {
            if (cbSlopes.SelectedItem != null)
            {
                var slope = cbSlopes.SelectedItem.ToString();
                slopeNames.Add(slope);
                cbChosenSlopes.Items.Add(slope);
                cbSlopes.Items.Remove(slope);
                cbChosenSlopes.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the cbDestinations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private async void cbDestinations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cbLifts.Items.Clear();
            this.cbSlopes.Items.Clear();
            slopes = await SkiAppDataSource.GetSlopesAsync();
            lifts = await SkiAppDataSource.GetLiftsAsync();
           
            if (cbDestinations.SelectedItem != null)
            {
                var selectedDestination = (string)cbDestinations.SelectedItem.ToString();
                if (destinations != null && destinations.Count != 0)
                {
                    var chosenDestination = destinations.FirstOrDefault(i => i.DestinationName == selectedDestination);

                    if (lifts != null && chosenDestination != null)
                    {
                        var destinationLifts = from lift in lifts
                                               where lift.LiftDestination.DestinationId == chosenDestination.DestinationId
                                               select lift.LiftName;
                        if (destinationLifts != null)
                        {
                            foreach (var l in destinationLifts)
                            {
                                cbLifts.Items.Add(l);
                            }
                        }
                        else
                        {
                            try
                            {
                                MessageDialog md = new MessageDialog("Kunne ikke hente informasjon. Sjekk internettkoblingen din og prøv på nytt!");
                                await md.ShowAsync();
                            }
                            catch (UnauthorizedAccessException)
                            {
                                //Dette skjer dersom brukeren får beskjed fra et annet sted om at noe gikk galt. 
                                //Trenger ikke gjøre noe med exception bare catche det så ikke programmet krasjer.
                            }
                        }
                    }

                    if (slopes != null)
                    {
                        var destinationSlopes = from slope in slopes
                                                where slope.SlopeDestination.DestinationId == chosenDestination.DestinationId
                                                select slope.SlopeName;
                        if (destinationSlopes != null)
                        {
                            foreach (var s in destinationSlopes)
                            {
                                cbSlopes.Items.Add(s);
                            }
                        }
                        else
                        {
                            try
                            {
                                MessageDialog md = new MessageDialog("Kunne ikke hente informasjon. Sjekk internettkoblingen din og prøv på nytt!");
                                await md.ShowAsync();
                            }
                            catch (UnauthorizedAccessException)
                            {
                                //Dette skjer dersom brukeren får beskjed fra et annet sted om at noe gikk galt. 
                                //Trenger ikke gjøre noe med exception bare catche det så ikke programmet krasjer.
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            MessageDialog md = new MessageDialog("Kunne ikke hente informasjon. Sjekk internettkoblingen din og prøv på nytt!");
                            await md.ShowAsync();
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
        /// Handles the Click event of the RemoveLift control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RemoveLift_Click(object sender, RoutedEventArgs e)
        {
            if (cbChosenLifts.SelectedItem != null)
            {
                var removedLift = cbChosenLifts.SelectedItem;
                cbChosenLifts.Items.Remove(removedLift);
                cbLifts.Items.Add(removedLift);
            }
        }

        /// <summary>
        /// Handles the Click event of the RemoveSlope control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RemoveSlope_Click(object sender, RoutedEventArgs e)
        {
            if (cbChosenSlopes.SelectedItem != null)
            {
                var removedSlope = cbChosenSlopes.SelectedItem;
                cbChosenSlopes.Items.Remove(removedSlope);
                cbSlopes.Items.Add(removedSlope);
            }
        }

        /// <summary>
        /// Handles the Click event of the StartPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void StartPage_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog md = new MessageDialog("Du blir logget ut dersom du går tilbake til startsiden. Vil du fortsette?");
            UICommand c1 = new UICommand("Fortsett");
            UICommand c2 = new UICommand("Avbryt");
            c1.Invoked = OkBtnClick;
            md.Commands.Add(c1);
            md.Commands.Add(c2);
            await md.ShowAsync();
        }

        /// <summary>
        /// Oks the back to start page button click.
        /// </summary>
        /// <param name="command">The command.</param>
        private void OkBtnClick(IUICommand command)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }
    }
}

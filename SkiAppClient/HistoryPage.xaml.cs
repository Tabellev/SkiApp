using SkiAppClient.Common;
using SkiAppClient.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace SkiAppClient
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class HistoryPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ObservableCollection<SkiDay> skiDays;
        private User user;
        private bool deleteSkiDayOk = true;
        private delegate string NumberOfTripsAsString();

        /// <summary>
        /// Gets the default view model.
        /// </summary>
        /// <value>
        /// The default view model.
        /// </value>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Gets the navigation helper.
        /// </summary>
        /// <value>
        /// The navigation helper.
        /// </value>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryPage"/> class.
        /// </summary>
        public HistoryPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Handles the LoadState event of the navigationHelper control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LoadStateEventArgs"/> instance containing the event data.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            skiDays = await SkiAppDataSource.GetSkiDaysAsync();
            user = (User)e.NavigationParameter;

            if ((skiDays != null && skiDays.Count != 0) && user != null)
            {
                foreach (var s in skiDays)
                {
                    if (s.SkiDayUser.UserId.Equals(user.UserId))
                    {
                        lbSkiDays.Items.Add(s.Date);
                    }
                }
            }
            else
            {
                try
                {
                    MessageDialog md = new MessageDialog("Kunne ikke hente dine skidager. Sjekk internettkoblingen din og prøv på nytt!");
                    await md.ShowAsync();
                }
                catch (UnauthorizedAccessException)
                {
                    //Dette skjer dersom brukeren får beskjed fra et annet sted om at noe gikk galt. 
                    //Trenger ikke gjøre noe med exception bare catche det så ikke programmet krasjer.
                }
            }
        }

        /// <summary>
        /// Handles the SaveState event of the navigationHelper control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SaveStateEventArgs"/> instance containing the event data.</param>
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
        /// Handles the Click event of the SeeSkiDay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SeeSkiDay_Click(object sender, RoutedEventArgs e)
        {
            var skiDayDate= (string)lbSkiDays.SelectedItem;
            SkiDay skiDay = null;

            if (skiDayDate != null)
            {
                if (skiDays != null && skiDays.Count != 0)
                {
                    foreach (var s in skiDays)
                    {
                        if (skiDayDate.Equals(s.Date))
                        {
                            skiDay = s;
                        }
                    }
                    if (!skiDay.Date.Equals(""))
                    {
                        tbDate.Text = skiDay.Date;
                    }
                    else
                    {
                        tbDate.Text = "-";
                    }

                    if (!skiDay.StartTime.Equals(""))
                    {
                        tbFromClock.Text = skiDay.StartTime;
                    }
                    else
                    {
                        tbFromClock.Text = "-";
                    }

                    if (!skiDay.StopTime.Equals(""))
                    {
                        tbToClock.Text = skiDay.StopTime;
                    }
                    else
                    {
                        tbToClock.Text = "-";
                    }
                    if (!skiDay.Equipment.Equals(""))
                    {
                        tbEquipment.Text = skiDay.Equipment;
                    }
                    else
                    {
                        tbEquipment.Text = "-";
                    }
                    if (!skiDay.Destination.Equals(""))
                    {
                        tbDestination.Text = skiDay.Destination;
                    }
                    else
                    {
                        tbDestination.Text = "-";
                    }

                    if (skiDay.NumberOfTrips != 0)
                    {
                        //Bruk av delegate
                        NumberOfTripsAsString numberOfTrips = new NumberOfTripsAsString(skiDay.NumberOfTrips.ToString);
                        tbTotalTrips.Text = numberOfTrips();
                    }
                    else
                    {
                        tbTotalTrips.Text = "-";
                    }

                    if (!skiDay.Comment.Equals(""))
                    {
                        tbComment.Text = skiDay.Comment;
                    }
                    else
                    {
                        tbComment.Text = "-";
                    }

                    var lifts = skiDay.Lifts;
                    var allLifts = "";
                    var slopes = skiDay.Slopes;
                    var allSlopes = "";

                    if (lifts != null && lifts.Count != 0)
                    {
                        foreach (var l in lifts)
                        {
                            if (lifts.First() == l)
                            {
                                allLifts = l.LiftName;
                            }
                            else
                            {
                                allLifts += (", " + l.LiftName);
                            }
                        }
                        tbLifts.Text = allLifts;
                    }
                    else
                    {
                        tbLifts.Text = "-";
                    }

                    if (slopes != null && slopes.Count != 0)
                    {
                        foreach (var s in slopes)
                        {
                            if (slopes.First() == s)
                            {
                                allSlopes = s.SlopeName;
                            }
                            else
                            {
                                allSlopes += (", " + s.SlopeName);
                            }
                        }
                        tbSlopes.Text = allSlopes;
                    }
                    else
                    {
                        tbSlopes.Text = "-";
                    }
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("Du må velge en skidag for å se på den!");
                await md.ShowAsync();
            }
        }

        /// <summary>
        /// Handles the Click event of the DeleteSkiDay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void DeleteSkiDay_Click(object sender, RoutedEventArgs e)
        {
            var skiDayDate = (string)lbSkiDays.SelectedItem;
            SkiDay skiDay = null;
            if (skiDayDate != null)
            {
                MessageDialog message = new MessageDialog("Er du sikker på at du vil slette den valgte skidagen?");
                UICommand c1 = new UICommand("Slett");
                UICommand c2 = new UICommand("Avbryt");
                c1.Invoked = DeleteBtnClick;
                c2.Invoked = CancelDeleteBtnClick;
                message.Commands.Add(c1);
                message.Commands.Add(c2);
                await message.ShowAsync();
               
                if (deleteSkiDayOk)
                {
                    if ((skiDays != null && skiDays.Count != 0) && user != null)
                    {
                        foreach (var s in skiDays)
                        {
                            if (skiDayDate.Equals(s.Date) && user.UserId.Equals(s.SkiDayUser.UserId))
                            {
                                skiDay = s;
                            }
                        }

                        if (skiDay != null)
                        {
                            await SkiAppDataSource.DeleteSkiDayAsync(skiDay.SkiDayId);
                            skiDays = await SkiAppDataSource.GetSkiDaysAsync();
                            lbSkiDays.Items.Remove(skiDayDate);
                            tbDate.Text = "-";
                            tbFromClock.Text = "-";
                            tbToClock.Text = "-";
                            tbEquipment.Text = "-";
                            tbDestination.Text = "-";
                            tbLifts.Text = "-";
                            tbSlopes.Text = "-";
                            tbTotalTrips.Text = "-";
                            tbComment.Text = "-";
                        }
                    }
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("Du må velge en skidag for å slette den!");
                await md.ShowAsync();
            }
        }

        /// <summary>
        /// OKs the delete skiDay click.
        /// </summary>
        /// <param name="command">The command.</param>
        private void DeleteBtnClick(IUICommand command)
        {
            deleteSkiDayOk = true;
        }

        /// <summary>
        /// Cancels the delete skiDay click.
        /// </summary>
        /// <param name="command">The command.</param>
        private void CancelDeleteBtnClick(IUICommand command)
        {
            deleteSkiDayOk = false;
        }

        /// <summary>
        /// Handles the Click event of the StartPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
         private async void StartPage_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog md = new MessageDialog("Du blir logget ut dersom du går tilbake til startsiden. Vil du fortsette?" );
            UICommand c1 = new UICommand("Fortsett");
            UICommand c2 = new UICommand("Avbryt");
            c1.Invoked = OkBtnClick;
            md.Commands.Add(c1);
            md.Commands.Add(c2);
            await md.ShowAsync();
        }

         /// <summary>
         /// Oks the go to start page click.
         /// </summary>
         /// <param name="command">The command.</param>
        private void OkBtnClick(IUICommand command)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }
    }
}

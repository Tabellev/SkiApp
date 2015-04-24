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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SkiAppClient
{
    public sealed partial class HistoryPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ObservableCollection<SkiDay> skiDays;
        private User user;

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public HistoryPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            skiDays = await SkiAppDataSource.GetSkiDaysAsync();
            user = (User)e.NavigationParameter;
            foreach (var s in skiDays)
            {
                if (s.SkiDayUser.UserId.Equals(user.UserId))
                {
                    lbSkiDays.Items.Add(s.Date);
                }
            }
        }

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

        private void SeeSkiDay_Click(object sender, RoutedEventArgs e)
        {
            var skiDayDate= (string)lbSkiDays.SelectedItem;
            SkiDay skiDay = null;

            if (skiDayDate != null)
            {
                foreach (var s in skiDays)
                {
                    if (skiDayDate.Equals(s.Date))
                    {
                        skiDay = s;
                    }
                }
                if (skiDay.Date != null)
                {
                    tbDate.Text = skiDay.Date;
                }
                else
                {
                    tbDate.Text = "-";
                }

                if (skiDay.StartTime != null)
                {
                    tbFromClock.Text = skiDay.StartTime;
                }
                else
                {
                    tbFromClock.Text = "-";
                }

                if (skiDay.StopTime != null)
                {
                    tbToClock.Text = skiDay.StopTime;
                }
                else
                {
                    tbToClock.Text = "-";
                }
                if (skiDay.Equipment != null)
                {
                    tbEquipment.Text = skiDay.Equipment;
                }
                else
                {
                    tbEquipment.Text = "-";
                }
                if (skiDay.Destination != null)
                {
                    tbDestination.Text = skiDay.Destination;
                }
                else
                {
                    tbDestination.Text = "-";
                }

                if (skiDay.NumberOfTrips != 0)
                {
                    tbTotalTrips.Text = skiDay.NumberOfTrips.ToString();
                }
                else
                {
                    tbTotalTrips.Text = "-";
                }

                if (skiDay.Comment != null)
                {
                    tbComment.Text = skiDay.Comment;
                }
                else
                {
                    tbComment.Text = "-";
                }

                var lifts = (ObservableCollection<Lift>)skiDay.Lifts;
                var allLifts = "";
                var slopes = skiDay.Slopes;
                var allSlopes = "";

                if (lifts != null)
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

                if (slopes != null)
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

        private async void DeleteSkiDay_Click(object sender, RoutedEventArgs e)
        {
            var skiDayDate = (string)lbSkiDays.SelectedItem;
            SkiDay skiDay = null;
            foreach (var s in skiDays)
            {
                if (skiDayDate.Equals(s.Date))
                {
                    skiDay = s;
                }
            }

            await SkiAppDataSource.DeleteSkiDayAsync(skiDay.SkiDayId);
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

        private void StartPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }
    }
}

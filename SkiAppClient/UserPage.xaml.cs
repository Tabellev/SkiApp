﻿using SkiAppClient.Common;
using SkiAppClient.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace SkiAppClient
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class UserPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private bool hasLogedOn = false;
        private SkiAppClient.LogOnPage.NavigationParameter navigationParameter;
        private User user;
        private UserText createUser;
        private UserText deleteUser;
        private UserText changePassword;
        private UserText skiDiary;

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

        public UserPage()
        {
            //Dårlig Maintainability Index, Class Coupling og Lines of code. Dette er autogenerert kode, så regner med at det er ok?
            this.InitializeComponent();

            // Setup the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            // Setup the logical page navigation components that allow
            // the page to only show one pane at a time.
            this.navigationHelper.GoBackCommand = new SkiAppClient.Common.RelayCommand(() => this.GoBack(), () => this.CanGoBack());
            this.itemListView.SelectionChanged += itemListView_SelectionChanged;

            // Start listening for Window size changes 
            // to change from showing two panes to showing a single pane
            Window.Current.SizeChanged += Window_SizeChanged;
            this.InvalidateVisualState();
        }

        private void SplitPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

      

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // Dette var eneste måten jeg fikk til å binde til det jeg skulle. Er det samme som alltid skal stå der. Ble veldig dårlig på Maintainability Index og lines of code.
            // Mulig jeg finner en bedre måte å gjøre det på etterhvert. Men måtte la det være sånn for å vise frem data nå.
            this.itemListView.SelectedItem = null;
            createUser = new UserText("Opprett bruker");
            deleteUser = new UserText("Slett bruker");
            changePassword = new UserText("Endre passord");
            //var logOff = new UserText("Logg ut");
            skiDiary = new UserText("Skidagbok");
            var userInfo = new ObservableCollection<UserText>();
            userInfo.Add(createUser);
            userInfo.Add(changePassword);
            userInfo.Add(deleteUser);
            //userInfo.Add(logOff);
            userInfo.Add(skiDiary);
            this.DefaultViewModel["Group"] = userInfo;

            if (e.NavigationParameter != null)
            {
                navigationParameter = (SkiAppClient.LogOnPage.NavigationParameter)e.NavigationParameter;
                hasLogedOn = navigationParameter.LogedOn;
                user = navigationParameter.LogedOnUser;

                if (hasLogedOn)
                {
                    logInName.Text = "Brukernavn: " + user.UserName;//LogOnPage.givenUserName;
                    logInButton.Content = "Logg ut";
                }
                else
                {
                    logInButton.Content = "Logg inn";
                    logInName.Text = "Ikke innlogget ";
                }
            }
            else if (e.NavigationParameter == null)
            {
                hasLogedOn = false;
                logInButton.Content = "Logg inn";
                logInName.Text = "Ikke innlogget ";
            }
            
            if (e.PageState == null)
            {
                this.itemListView.SelectedItem = null;
               
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemListView.SelectedItem = null;
                }
            }
            else
            {
                this.itemListView.SelectedItem = null;
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
            if (this.itemsViewSource.View != null)
            {
                // TODO: Derive a serializable navigation parameter and assign it to
                //       pageState("SelectedItem")
                //this.itemListView.SelectedItem = null;

            }
        }

        #region Logical page navigation

        // The split page isdesigned so that when the Window does have enough space to show
        // both the list and the dteails, only one pane will be shown at at time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        private const int MinimumWidthForSupportingTwoPanes = 768;

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <returns>True if the window should show act as one logical page, false
        /// otherwise.</returns>
        private bool UsingLogicalPageNavigation()
        {
            return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes;
        }

        /// <summary>
        /// Invoked with the Window changes size
        /// </summary>
        /// <param name="sender">The current Window</param>
        /// <param name="e">Event data that describes the new size of the Window</param>
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
        //Kan hende jeg skal bruke de senere
        private void  itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.itemListView.SelectedItem != null)
            {
                var selected = (UserText)this.itemListView.SelectedItem;
                var selectedType = selected.Information;
                
                if (selectedType.Equals(createUser.Information))
                {
                    this.Frame.Navigate(typeof(CreateUserPage));
                }
                else if(selectedType.Equals(changePassword.Information))
                {
                    this.Frame.Navigate(typeof(ChangePasswordPage));
                }
                else if (selectedType.Equals(deleteUser.Information))
                {
                    this.Frame.Navigate(typeof(DeleteUserPage));
                }
                else if (selectedType.Equals(skiDiary.Information))
                {
                    if (user != null)
                    {
                        this.Frame.Navigate(typeof(SkiDayPage), user);
                    }
                    else
                    {
                        tbWarning.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    } 
                }
            }
            
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }


        private bool CanGoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                return true;
            }
            else
            {
                return this.navigationHelper.CanGoBack();
            }
        }
        private void GoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                this.itemListView.SelectedItem = null;
            }
            else
            {
                this.itemListView.SelectedItem = null;
                this.navigationHelper.GoBack();
            }
        }

        private void InvalidateVisualState()
        {
            var visualState = DetermineVisualState();
            VisualStateManager.GoToState(this, visualState, false);
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        private string DetermineVisualState()
        {
            if (!UsingLogicalPageNavigation())
                return "PrimaryView";

            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null;

            return logicalPageBack ? "SinglePane_Detail" : "SinglePane";
        }

        #endregion

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
            this.itemListView.SelectedItem = null;
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.itemListView.SelectedItem = null;
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (hasLogedOn)
            {
                hasLogedOn = false;
                logInName.Text = "Ikke innlogget";
                logInButton.Content = "Logg inn";
            }
            else
            {
                hasLogedOn = false;
                logInButton.Content = "Logg inn";
                logInName.Text = "Ikke innlogget";
                this.Frame.Navigate(typeof(LogOnPage));
            }
            
        }

        private void StartPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }
        
    }
}

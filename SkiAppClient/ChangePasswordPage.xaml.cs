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
    public sealed partial class ChangePasswordPage : Page
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
        /// Initializes a new instance of the <see cref="ChangePasswordPage"/> class.
        /// </summary>
        public ChangePasswordPage()
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
        /// Handles the Click event of the ChangePassword control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var users = await SkiAppDataSource.GetUsersAsync();
            var givenUserName = userName.Text;
            var givenPassword = oldPassword.Password;
            var givenNewPassword = newPassword.Password;
            User currentUser = null;

            if (users != null)
            {
                foreach (var user in users)
                {
                    if (givenUserName.Equals(user.UserName))
                    {
                        currentUser = user;
                    }
                }

                if (currentUser != null)
                {
                    if (givenPassword.Equals(currentUser.Password))
                    {
                        if (!givenNewPassword.Equals(""))
                        {
                            await SkiAppDataSource.ChangePasswordAsync(currentUser, givenNewPassword);
                            MessageDialog md = new MessageDialog("Passord endret for " + givenUserName);
                            await md.ShowAsync();
                            this.Frame.Navigate(typeof(UserPage));
                        }
                        else
                        {
                            MessageDialog md = new MessageDialog("Nytt passord må fylles ut!");
                            await md.ShowAsync();
                        }
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog("Feil brukernavn eller passord!");
                        await md.ShowAsync();
                        userName.Text = String.Empty;
                        oldPassword.Password = String.Empty;
                        newPassword.Password = String.Empty;
                    }
                }
                else
                {
                    MessageDialog md = new MessageDialog("Feil brukernavn eller passord! Prøv på nytt!");
                    await md.ShowAsync();
                    userName.Text = String.Empty;
                    oldPassword.Password = String.Empty;
                    newPassword.Password = String.Empty;
                }
            }
            else
            {
                try
                {
                    MessageDialog md = new MessageDialog("Passordet ble ikke endret. Sjekk internettkoblingen din og prøv på nytt!");
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
         /// OKs the go to start page click.
         /// </summary>
         /// <param name="command">The command.</param>
        private void OkBtnClick(IUICommand command)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }
    }
}

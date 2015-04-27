using SkiAppClient.Common;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SkiAppClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogOnPage : Page
    {
        private NavigationHelper navigationHelper;
        private string givenUserName;
        private bool isLogedOn;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOnPage"/> class.
        /// </summary>
        public LogOnPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
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
        /// Handles the LoadState event of the navigationHelper control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LoadStateEventArgs"/> instance containing the event data.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Handles the Click event of the LoggOn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void LoggOn_Click(object sender, RoutedEventArgs e)
        {
            var users = await SkiAppDataSource.GetUsersAsync();
            givenUserName = userName.Text;
            var givenPassword = password.Password;
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
                        isLogedOn = true;
                        MessageDialog md = new MessageDialog("Du er nå logget inn som " + givenUserName);
                        await md.ShowAsync();
                        userName.Text = String.Empty;
                        password.Password = String.Empty;
                        NavigationParameters navigationParameter = new NavigationParameters(isLogedOn, currentUser);
                        this.Frame.Navigate(typeof(UserPage), navigationParameter);
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog("Feil brukernavn eller passord! Prøv på nytt!");
                        await md.ShowAsync();
                        isLogedOn = false;

                        userName.Text = String.Empty;
                        password.Password = String.Empty;
                    }
                }
                else
                {
                    MessageDialog md = new MessageDialog("Feil brukernavn eller passord! Prøv på nytt!");
                    await md.ShowAsync();
                }
            }
            else
            {
                try
                {
                    MessageDialog md = new MessageDialog("Du ble ikke logget inn. Sjekk internettkoblingen din og prøv på nytt!");
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
        private void StartPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }

        /// <summary>
        /// Handles the Click event of the Cancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserPage));
        }
    }
}

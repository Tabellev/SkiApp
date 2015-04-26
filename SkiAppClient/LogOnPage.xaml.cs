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
       
        public LogOnPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            isLogedOn = false;
        }

        private async void LogOn_Click(object sender, RoutedEventArgs e)
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
        }


        //Bruker en struct for å kunne sende med to verdier som navigationpatameter.
        //Trenger ikke masse funksjonalitet, så derfor velger jeg en struct isteden for en klasse.
        private struct NavigationParametesUserPage
        {
            

        }

        private void StartPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserPage));
        }
        

    }
}

using SkiAppClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiAppClient
{
    //Bruker en struct når jeg skal sende mer enn et parameter fra en side til en annen. De er av ulik type, så kan ikke bruke liste.
    //Trenger ikke masse funksjonalitet, så derfor bruker jeg struct isteden for klasse.
   
    /// <summary>
    /// A struct that has a bool and a User that is used as navigation parameters between pages.
    /// </summary>
    public struct NavigationParameters
    {
        private bool LoggedOn;

        private User LoggedOnUser;
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationParameters"/> struct.
        /// </summary>
        /// <param name="loggedOn">if set to <c>true</c> [logged on].</param>
        /// <param name="user">The user.</param>
        public NavigationParameters(bool loggedOn, User user)
        {
            LoggedOn = loggedOn;
            LoggedOnUser = user;
        }

        /// <summary>
        /// Gets the logged on user.
        /// </summary>
        /// <returns>
        /// User
        /// </returns>
        public User GetLoggedOnUser()
        {
            return LoggedOnUser;
        }

        /// <summary>
        /// Gets the LoggedOn value true or false.
        /// </summary>
        /// <returns>bool LoggedOn</returns>
        public bool GetLoggedOn()
        {
            return LoggedOn;
        }

        /// <summary>
        /// Sets the logged on user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void SetLoggedOnUser(User user)
        {
            this.LoggedOnUser = user;
        }

        /// <summary>
        /// Sets the LoggedOn to true of false.
        /// </summary>
        /// <param name="loggedOn">if set to <c>true</c> [logged on].</param>
        public void SetLoggedOn(bool loggedOn)
        {
            this.LoggedOn = loggedOn;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
             if (!(obj is NavigationParameters))                
                return false;

             return Equals((NavigationParameters)obj);    
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true or false</returns>
        public bool Equals(NavigationParameters other)
        {
            if (LoggedOnUser != other.LoggedOnUser)
                return false;

            return LoggedOn == other.LoggedOn;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="navigationParameters1">The navigation parameters1.</param>
        /// <param name="navigationParameters2">The navigation parameters2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(NavigationParameters navigationParameters1, NavigationParameters navigationParameters2)
        {
            return navigationParameters1.Equals(navigationParameters2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="navigationParameters1">The navigation parameters1.</param>
        /// <param name="navigationParameters2">The navigation parameters2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(NavigationParameters navigationParameters1, NavigationParameters navigationParameters2)
        {
            return !navigationParameters1.Equals(navigationParameters2);
        } 
    }
}

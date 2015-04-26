using SkiAppClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiAppClient
{
    public struct NavigationParameters
    {
        public bool LoggedOn;

            public User LoggedOnUser;
            public NavigationParameters(bool loggedOn, User user)
            {
                LoggedOn = loggedOn;
                LoggedOnUser = user;
            }
    }
}

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

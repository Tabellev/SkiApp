using SkiAppClient.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace SkiAppClient
{
    public class Parser
    {
        public async static Task<string> getOpeningHours(Destination2 destination)
        {
            string html = "Startverdi";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(destination.getUrl()[0]);
            //try
            WebResponse x = await req.GetResponseAsync();
            HttpWebResponse res = (HttpWebResponse)x;

            if (res != null)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = res.GetResponseStream();

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                        
                    }
                }
                else
                {
                    html = "Ikke OK";
                }
                res.Dispose();
            }

            return html;
        }
    }
}

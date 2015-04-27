using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace SkiAppClient.DataModel
{
    public sealed class SkiAppDataSource
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="SkiAppDataSource"/> class from being created.
        /// </summary>
        private SkiAppDataSource() { }

        /// <summary>
        /// Gets the destinations asynchronous.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        //Not appropriate
        public static async Task<ObservableCollection<Destination>> GetDestinationsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var result = await client.GetAsync("api/Destinations");
                    if (result.IsSuccessStatusCode)
                    {
                        var resultStream = await result.Content.ReadAsStreamAsync();
                        var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Destination>));
                        ObservableCollection<Destination> destinations = (ObservableCollection<Destination>)serializer.ReadObject(resultStream);
                        return destinations;
                    }
                    else
                    {
                        MessageDialog d = new MessageDialog("Noe gikk galt! Sjekk internettkoblingen din og start appen på nytt!");
                        await d.ShowAsync();
                        //Application.Current.Exit();
                        return null;
                    }
                }
                catch (TaskCanceledException)
                {
                    MessageDialog d = new MessageDialog("Noe gikk galt! Sjekk internettkoblingen din og start appen på nytt!");
                    d.ShowAsync();
                    return null;
                }
            }
        }

        /*public static async Task<Destination> GetOneDestinationAsync(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Destinations/" + id);

                if (result.IsSuccessStatusCode)
                {
                    var resultStream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(Destination));
                    Destination destination = (Destination)serializer.ReadObject(resultStream);

                    return destination;
                }
                else
                {
                    return null;
                }
            }
        }*/

        /// <summary>
        /// Gets the opening hours asynchronous.
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<OpeningHours>> GetOpeningHoursAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/OpeningHours");

                if (result.IsSuccessStatusCode)
                {
                    var resultStream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<OpeningHours>));
                    ObservableCollection<OpeningHours> openingHours = (ObservableCollection<OpeningHours>)serializer.ReadObject(resultStream);
                    return openingHours;
                }
                else
                {
                    MessageDialog d = new MessageDialog("Noe gikk galt! Sjekk internettkoblingen din og start appen på nytt!");
                    await d.ShowAsync();
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the users asynchronous.
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<User>> GetUsersAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Users");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<User>));
                    ObservableCollection<User> users = (ObservableCollection<User>)serializer.ReadObject(resultSTream);
                    return users;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Adds a user asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static async Task AddUserAsync(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                User newUser = new User() { UserName = userName, Password = password };
                var jsonSerializer = new DataContractJsonSerializer(typeof(User));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newUser);
                stream.Position = 0;
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Users", content);

                if (!response.IsSuccessStatusCode)
                {
                    MessageDialog md = new MessageDialog("Bruker ble ikke opprett. Sjekk internettkoblingen din og prøv på nytt!");
                    await md.ShowAsync();
                }
            }
        }

        /// <summary>
        /// Deletes a user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static async Task DeleteUserAsync(int userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync("api/Users/" + userId);
                if (!response.IsSuccessStatusCode)
                {
                    MessageDialog md = new MessageDialog("Bruker ble ikke slettet. Sjekk internettkoblingen din og prøv på nytt!");
                    await md.ShowAsync();
                }
            }
        }

        /// <summary>
        /// Adds a ski day asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="date">The date.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="stopTime">The stop time.</param>
        /// <param name="equipment">The equipment.</param>
        /// <param name="numberOfTrips">The number of trips.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="lifts">The lifts.</param>
        /// <param name="slopes">The slopes.</param>
        /// <returns></returns>
        public static async Task AddSkiDayAsync(User user, string destination, string date, string startTime, string stopTime, string equipment, int numberOfTrips, string comment, 
            ObservableCollection<Lift> lifts, ObservableCollection<Slope> slopes)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                SkiDay newSkiDay = new SkiDay() { SkiDayUser = user, Destination = destination, Date = date, StartTime = startTime, StopTime = stopTime, Equipment = equipment, NumberOfTrips = numberOfTrips, Comment = comment, Lifts = lifts, Slopes = slopes };
                
                //var jsonSerializerSettings = new DataContractJsonSerializerSettings();
                var jsonSerializer = new DataContractJsonSerializer(typeof(SkiDay));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newSkiDay);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/SkiDays", content);

                if (!response.IsSuccessStatusCode)
                {
                    MessageDialog md = new MessageDialog("Skidag ble ikke lagret. Sjekk internettkoblingen din og prøv på nytt!");
                    await md.ShowAsync();
                }
            }
        }

        /// <summary>
        /// Gets the ski days asynchronous.
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<SkiDay>> GetSkiDaysAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/SkiDays");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<SkiDay>));
                    ObservableCollection<SkiDay> skiDays = (ObservableCollection<SkiDay>)serializer.ReadObject(resultSTream);
                    return skiDays;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Changes the password asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static async Task ChangePasswordAsync(User user, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                User updatetUser = new User();
                updatetUser.UserId = user.UserId;
                updatetUser.UserName = user.UserName;
                updatetUser.Password = password;
                var jsonSerializer = new DataContractJsonSerializer(typeof(User));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, updatetUser);
                stream.Position = 0;
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync("api/users/" + updatetUser.UserId, content);

                if (!response.IsSuccessStatusCode)
                {
                    MessageDialog md = new MessageDialog("Passord ble ikke endret. Sjekk internettkoblingen din og prøv på nytt!");
                    await md.ShowAsync();
                }
            }
        }


        /// <summary>
        /// Changes the ski day asynchronous.
        /// </summary>
        /// <param name="skiDay">The ski day.</param>
        /// <returns></returns>
        public static async Task ChangeSkiDayAsync(SkiDay skiDay)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                SkiDay updatedSkiDay = new SkiDay() {SkiDayId = skiDay.SkiDayId, SkiDayUser = skiDay.SkiDayUser, Date = skiDay.Date, Comment = skiDay.Comment, Destination = skiDay.Destination, 
                Equipment = skiDay.Equipment, Lifts = null, Slopes = null};

                var jsonSerializer = new DataContractJsonSerializer(typeof(SkiDay));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, updatedSkiDay);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                //var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
                var response = await client.PutAsync("api/SkiDays/" + skiDay.SkiDayId, content);

                if (!response.IsSuccessStatusCode)
                {
                    MessageDialog md = new MessageDialog("Skidag ble ikke endret. Sjekk internettkoblingen din og prøv på nytt!");
                    await md.ShowAsync();
                }
            }
        }

        /// <summary>
        /// Deletes a ski day asynchronous.
        /// </summary>
        /// <param name="skiDayId">The ski day identifier.</param>
        /// <returns></returns>
        public static async Task DeleteSkiDayAsync(int skiDayId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
   
                var response = await client.DeleteAsync("api/SkiDays/" + skiDayId);

                if (!response.IsSuccessStatusCode)
                {
                    MessageDialog md = new MessageDialog("Skidag ble ikke slettet. Sjekk internettkoblingen din og prøv på nytt!");
                    await md.ShowAsync();
                }
            }
        }


        /// <summary>
        /// Gets the lifts asynchronous.
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Lift>> GetLiftsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Lifts");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Lift>));
                    ObservableCollection<Lift> lifts = (ObservableCollection<Lift>)serializer.ReadObject(resultSTream);
                    return lifts;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the slopes asynchronous.
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Slope>> GetSlopesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/Slopes");

                if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Slope>));
                    ObservableCollection<Slope> slopes = (ObservableCollection<Slope>)serializer.ReadObject(resultSTream);
                    return slopes;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

    


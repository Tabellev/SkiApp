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

namespace SkiAppClient.DataModel
{
    public sealed class SkiAppDataSource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //Må ha med setter eller så får jeg feilmelding.
        //public ObservableCollection<Destination> Destinations { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        //Not appropriate
        public static async Task<ObservableCollection<Destination>> GetDestinationAsync() 
        {   using (var client = new HttpClient())
            {
              client.BaseAddress = new Uri("http://localhost:2219/"); 
              client.DefaultRequestHeaders.Accept.Clear(); 
              client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); 
            
              var result = await client.GetAsync("api/Destinations"); 
            
              if (result.IsSuccessStatusCode) 
              { var resultStream = await result.Content.ReadAsStreamAsync(); 
                var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Destination>)); 
                ObservableCollection<Destination> destinations = (ObservableCollection<Destination>)serializer.ReadObject(resultStream); 
                return destinations; 
              } else 
              { 
                return null; 
              } 
        } 
      }

        public static async Task<Destination> GetOneDestinationAsync(int id)
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
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        //Not appropriate
        public static async Task<ObservableCollection<DestinationInfoType>> GetDestinationInfoTypeAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync("api/DestinationInfoTypes");

                if (result.IsSuccessStatusCode)
                {
                    var resultStream = await result.Content.ReadAsStreamAsync();
                    var serializer = new DataContractJsonSerializer(typeof(ObservableCollection<DestinationInfoType>));
                    ObservableCollection<DestinationInfoType> destinationInfoTypes = (ObservableCollection<DestinationInfoType>)serializer.ReadObject(resultStream);
                    return destinationInfoTypes;
                }
                else
                {
                    return null;
                }
            } 
        }

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
                    return null;
                }
            }
        }

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

        public static async Task AddUserAsync(string username, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                User newUser = new User() { UserName = username, Password = password };
                var jsonSerializer = new DataContractJsonSerializer(typeof(User));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newUser);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Users", content);

                response.EnsureSuccessStatusCode();
            }
        }

        public static async Task DeleteUserAsync(int userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync("api/Users/" + userId);

                response.EnsureSuccessStatusCode();
            }
        }

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

                response.EnsureSuccessStatusCode();
            }
        }

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
                //var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
                var jsonSerializer = new DataContractJsonSerializer(typeof(User));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, updatetUser);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                //var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
                var response = await client.PutAsync("api/users/" + updatetUser.UserId, content);

                response.EnsureSuccessStatusCode(); // Throw an exception if something went wrong

                // a smarter approach would be to update the element (remove/add is brute force)
                //_academiaDataSource._courses.Remove(_academiaDataSource._courses.First(c => c.CourseId == aCourse.CourseId));
                //_academiaDataSource._courses.Add(aCourse);
            }
        }

       
        public static async Task ChangeSkiDayAsync(SkiDay skiDay)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                SkiDay updatedSkiDay = new SkiDay() {SkiDayId = skiDay.SkiDayId, SkiDayUser = skiDay.SkiDayUser, Date = skiDay.Date, Comment = skiDay.Comment, Destination = skiDay.Destination, 
                Equipment = skiDay.Equipment, Lifts = null, Slopes = null};

                //var jsonSerializerSettings = new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat(dateTimeFormat) };
                var jsonSerializer = new DataContractJsonSerializer(typeof(SkiDay));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, updatedSkiDay);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                //var client = new HttpClient { BaseAddress = new Uri(RestServiceUrl) };
                var response = await client.PutAsync("api/SkiDays/" + skiDay.SkiDayId, content);

                response.EnsureSuccessStatusCode(); // Throw an exception if something went wrong

                // a smarter approach would be to update the element (remove/add is brute force)
                //_academiaDataSource._courses.Remove(_academiaDataSource._courses.First(c => c.CourseId == aCourse.CourseId));
                //_academiaDataSource._courses.Add(aCourse);
            }
        }

        public static async Task DeleteSkiDayAsync(int skiDayId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2219/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
   
                var response = await client.DeleteAsync("api/SkiDays/" + skiDayId);

                response.EnsureSuccessStatusCode();
            }
        }


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

    


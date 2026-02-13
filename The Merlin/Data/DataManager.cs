using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using The_Merlin.Models;
using The_Merlin.Services;

namespace The_Merlin.Data
{
    public class DataManager
    {
        //public readonly SQLiteConnection dbConnection;
        //public DataManager()
        //{
        //    dbConnection = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "themerlin.db3"));
        //    Debug.WriteLine($"Database path: {Path.Combine(FileSystem.AppDataDirectory, "themerlin.db3")}");
        //    dbConnection.CreateTable<TodoItem>();
        //    dbConnection.CreateTable<TimelineItem>();
        //    dbConnection.CreateTable<TodoDefItem>();
        //    dbConnection.CreateTable<DayItem>();
        //}

        public HttpClient HttpClient { get; set; }
        public string Url;
        public DataManager()
        {

            //Url = "https://www.cfogger.me/";
            //PC url
            Url = "https://localhost:44387/";
            //Laptop url
            //Url = "http://localhost:50173/";
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpClient.Timeout = TimeSpan.FromSeconds(15);
        }


        public async Task<object> resolveRespond(string url, string JsonContent = "nulloğlunull")
        {
            object result = null;
            try
            {
                HttpResponseMessage response;
                LoadingService.Instance.IsLoading = true; // Ekran kararır, çark döner
                try
                {
                    if (JsonContent == "nulloğlunull")
                        response = await HttpClient.GetAsync(Url + url);
                    else
                        response = await HttpClient.PostAsync(Url + url, new StringContent(JsonContent, Encoding.UTF8, "application/json"));
                }
                finally
                {
                    LoadingService.Instance.IsLoading = false; // Her şey normale döner
                }

                if (response.IsSuccessStatusCode)
                {
                    string? resp = await response.Content.ReadAsStringAsync();
                    MobileResult? mResult = JsonConvert.DeserializeObject<MobileResult>(resp);
                    if (mResult.Result)
                        result = mResult.Data;
                    Debug.WriteLine("On DB: " + mResult.Message);
                }
                else
                {
                    Debug.WriteLine($"Response Fail: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exc: " + e.Message);
            }
            return result;
        }
    }
}

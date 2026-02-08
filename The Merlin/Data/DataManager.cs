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
        bool isDebug = false;
        public string Url;
        BusyService _busy;
        public DataManager(BusyService busy)
        {
            Url = isDebug ? "https://localhost:44387/" : "https://www.cfogger.me/";
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpClient.Timeout = TimeSpan.FromSeconds(15);
            _busy = busy;
        }

        public async Task<object> resolveRespond(string url, string JsonContent = "nulloğlunull")
        {
            try
            {
                _busy.Begin();
                HttpResponseMessage response;
                if (JsonContent == "nulloğlunull")
                    response = await HttpClient.GetAsync(Url + url);
                else
                    response = await HttpClient.PostAsync(Url + url, new StringContent(JsonContent, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    MobileResult mResult = JsonConvert.DeserializeObject<MobileResult>(resp);
                    if (mResult.Result)
                        return mResult.Data;
                    else
                        Debug.WriteLine(mResult.Message);
                }
                else
                {
                    Debug.WriteLine($"Response: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exc: " + e.Message);
            }
            finally
            {
                _busy.End();
            }
            return null;
        }
    }
}

using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RMA.Model;

namespace RMA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewMain : ContentPage
    {
        bool isOn;
        bool backlight;
        bool isConnected;
        string API_URL;

        public ViewMain()
        {
            InitializeComponent();

            isConnected = CrossConnectivity.Current.IsConnected;

            #if __ANDROID__
                Android.Content.ISharedPreferences prefs;
                prefs = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
                backlight = prefs.GetBoolean("pref_backlight", true);
                API_URL = prefs.GetString("pref_api_url", "http://192.168.1.100");
            #endif

            #if __IOS__
                 backlight = Foundation.NSUserDefaults.StandardUserDefaults.BoolForKey("pref_backlight");
                 API_URL = Foundation.NSUserDefaults.StandardUserDefaults.StringForKey("pref_api_key");
            #endif

            if (isConnected)
            {
                RefreshDataAsync();
            }
            else
            {
                DisplayAlert("Problem s vezom", "Nemoguće povezivanje s mrežom. Provjerite vezu", "OK");
            }
        }

        private async void RefreshDataAsync()
        {
            Data data = await GetDataFromAPI();

            currentTempLbl.Text = data.curTemp.ToString() + " °C";
            wantedTempInput.Text += data.wanTemp.ToString();
            onOffSwitch.IsToggled = data.isOn;
        }

        private async Task<Data> GetDataFromAPI()
        {
            Data data = new Data();

            try
            {
                var uri = new Uri(API_URL + "/api.php");
                HttpClient client = new HttpClient();
                client.Timeout = new System.TimeSpan(0, 0, 0, 1, 0);
            
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseJSON = await response.Content.ReadAsStringAsync();

                data = JsonConvert.DeserializeObject<Data>(responseJSON);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (isConnected)
            {
                var client = new HttpClient();

                Data data = new Data
                {
                    wanTemp = Convert.ToInt32(wantedTempInput.Text),
                    isOn = this.isOn,
                    backlight = this.backlight
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(API_URL + "/reciver.php", content);
                    #if __ANDROID__
                        Android.Widget.Toast.MakeText((Android.App.Activity)Forms.Context, "Podaci poslani", Android.Widget.ToastLength.Long).Show();
                    #endif
                }
                catch
                {
                    //Toast.MakeText((Android.App.Activity)Forms.Context, "Dogodila se pogreška pri slanju podataka", ToastLength.Long).Show();
                    await DisplayAlert("Pogreška", "Dogodila se pogreška pri slanju podataka. Provjerite jeste li povezani s poslužiteljem", "OK");
                }
            }
        }

        private void onOffSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Xamarin.Forms.Switch onOffSwitch = this.FindByName<Xamarin.Forms.Switch>("onOffSwitch");
            isOn = onOffSwitch.IsToggled;
        }
    }
}

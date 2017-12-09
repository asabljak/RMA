using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RMA.Model;
using Android.Content;
using Android.Preferences;



namespace RMA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewMain : ContentPage
    {
        bool isOn;
        bool backlight;
        ISharedPreferences prefs;
        bool isConnected;
        string API_URL;

        public ViewMain()
        {
            InitializeComponent();

            isConnected = CrossConnectivity.Current.IsConnected;
            prefs = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            backlight = prefs.GetBoolean("pref_backlight", true);
            API_URL = prefs.GetString("pref_api_url", "http://192.168.1.100");

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

            Label currentTempLbl = this.FindByName<Label>("currentTempLbl");
            Entry wantedTempInput = this.FindByName<Entry>("wantedTempInput");

            currentTempLbl.Text = data.curTemp.ToString();
            wantedTempInput.Text += data.wanTemp.ToString();
        }

        private async Task<Data> GetDataFromAPI()
        {
            Data data = new Data();

            try
            {
                var uri = new Uri(API_URL + "/api.php");
                HttpClient client = new HttpClient();
                client.Timeout = new System.TimeSpan(0, 0, 0, 1, 0);
                // client.DefaultRequestHeaders.Host = "pizzaboy.de";
                //client.BaseAddress = uri;
                var response = await client.GetAsync(uri);

                response.EnsureSuccessStatusCode();
                var responseJSON = await response.Content.ReadAsStringAsync();

                data = JsonConvert.DeserializeObject<Data>(responseJSON);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //   String innerEx = e.InnerException.Message;//network is unreachable

                //data.curTemp = 22.5;
                //data.isOn = true;
                //data.temp = 40;

            }

            return data;

        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (isConnected)
            {
                var client = new HttpClient();
                Label currentTempLbl = this.FindByName<Label>("currentTempLbl");
                Entry wantedTempInput = this.FindByName<Entry>("wantedTempInput");
                //Switch onOffSwitch = this.FindByName<Switch>("onOffSwitch");
                Switch backlightSwitch = this.FindByName<Switch>("backlightSwitch");

                Data data = new Data
                {
                    wanTemp = Convert.ToInt32(wantedTempInput.Text),
                    isOn = isOn,
                    backlight = this.backlight
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(API_URL + "/reciver.php", content);

            }

        }

        private void backlightSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            //ISharedPreferencesEditor editor = prefs.Edit();
            //editor.PutBoolean("pref_key_backlight", (sender as Switch).IsToggled);
        }

        private void onOffSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Switch onOffSwitch = this.FindByName<Switch>("onOffSwitch");
            isOn = onOffSwitch.IsToggled;
        }
    }
}

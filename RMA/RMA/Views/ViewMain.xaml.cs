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

namespace RMA.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewMain : ContentPage
	{
        bool isOn;
        bool backlight;
        //ISharedPreferences prefs;
        bool isConnected;

        public ViewMain()
        {
            InitializeComponent();

            isConnected = CrossConnectivity.Current.IsConnected;
            if (isConnected)
            {
                RefreshDataAsync();
            }
            else
            {
                DisplayAlert("Problem s vezom", "Nemoguće povezivanje s mrežom. Provjerite vezu", "OK");
            }
            //prefs = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            //backlight = prefs.GetBoolean("pref_key_backlight", true);
        }

        private async void RefreshDataAsync()
        {
            Data data = await GetDataFromAPI();

            Label currentTempLbl = this.FindByName<Label>("currentTempLbl");
            Entry wantedTempInput = this.FindByName<Entry>("wantedTempInput");

            currentTempLbl.Text = data.curTemp.ToString();
            wantedTempInput.Text += data.temp.ToString();
        }

        private async Task<Data> GetDataFromAPI()
        {
            Data data = new Data();

            try
            {
                // throw new Exception("PAM!");
                var uri = new Uri("http://192.168.1.100/api.php");
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

                Data data = new Data();
                data.temp = Convert.ToInt32(wantedTempInput.Text);
                data.isOn = isOn;
                data.backlight = backlightSwitch.IsToggled;

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("http://192.168.1.100/reciver.php", content);

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

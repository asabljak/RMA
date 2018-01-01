using System;
using System.Collections.Generic;
using Xamarin.Forms;
using RMA.Views;
using RMA.MenuItem;

namespace RMA
{
	public partial class MainPage : MasterDetailPage
	{
        List<MasterMenuItem> MenuList { get; set; }
        String API_URL;

        public MainPage()
		{
			InitializeComponent();

            #if __ANDROID__
                Android.Content.ISharedPreferences prefs;
                prefs = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
                API_URL = prefs.GetString("pref_api_url", "http://192.168.1.100");
            #endif

            #if __IOS__
                 API_URL = Foundation.NSUserDefaults.StandardUserDefaults.StringForKey("pref_api_key");
            #endif

            MenuList = new List<MasterMenuItem>();
            MenuList.Add(new MasterMenuItem("Settings", null, typeof(SettingsPage)));
            navigationDrawerList.ItemsSource = MenuList;

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ViewMain)));
            this.BindingContext = new
            {
                Header = "",
                Image = API_URL + "/rpi-logo.png",
                Footer = "HeatMe"
            };
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterMenuItem)e.SelectedItem;
            Type page = item.TargetType;
            Navigation.PushModalAsync(new NavigationPage((Page)Activator.CreateInstance(page)));

            IsPresented = false;
        }
    }
}


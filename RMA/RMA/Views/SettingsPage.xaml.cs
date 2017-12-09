using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Preferences;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RMA.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        ISharedPreferences prefs;
        ISharedPreferencesEditor editor;

        public SettingsPage ()
		{
			InitializeComponent ();

            prefs = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            backlightSwitch.IsToggled = prefs.GetBoolean("pref_backlight", true);
            apiUrl.Text = prefs.GetString("pref_api_key", "http://192.168.1.100");
        }

        protected override bool OnBackButtonPressed()
        {
            editor = prefs.Edit();
            editor.PutBoolean("pref_backlight", backlightSwitch.IsToggled);
            editor.PutString("pref_api_key", apiUrl.Text);
            editor.Apply();

            Navigation.PushAsync(new ViewMain());
            return true;
        }
    }
}
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RMA.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
#if __ANDROID__
        Android.Content.ISharedPreferences prefs;
        Android.Content.ISharedPreferencesEditor editor;
#endif

        public SettingsPage ()
		{
			InitializeComponent ();

#if __ANDROID__
            prefs = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            backlightSwitch.IsToggled = prefs.GetBoolean("pref_backlight", true);
            apiUrl.Text = prefs.GetString("pref_api_key", "http://192.168.1.100");
#endif

#if __IOS__
            backlightSwitch.IsToggled = Foundation.NSUserDefaults.StandardUserDefaults.BoolForKey("pref_backlight");
            apiUrl.Text = Foundation.NSUserDefaults.StandardUserDefaults.StringForKey("pref_api_key");
#endif
        }

        protected override bool OnBackButtonPressed()
        {
#if __ANDROID__
                editor = prefs.Edit();
                editor.PutBoolean("pref_backlight", backlightSwitch.IsToggled);
                editor.PutString("pref_api_key", apiUrl.Text);
                editor.Apply();
#endif

#if __IOS__
                Foundation.NSUserDefaults.StandardUserDefaults.SetBool(backlightSwitch.IsToggled, "pref_backlight");
                Foundation.NSUserDefaults.StandardUserDefaults.SetString(apiUrl.Text, "pref_api_key");
                Foundation.NSUserDefaults.StandardUserDefaults.Synchronize();
#endif

            Navigation.PushAsync(new ViewMain());
            return true;
        }
    }
}
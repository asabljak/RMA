using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using RMA.MenuItem;
using RMA.Views;
using Android.Content;
using Android.Preferences;

namespace RMA
{
	public partial class MainPage : MasterDetailPage
	{
        List<MasterMenuItem> menuList { get; set; }
        ISharedPreferences prefs;

        public MainPage()
		{
			InitializeComponent();

            menuList = new List<MasterMenuItem>();
            menuList.Add(new MasterMenuItem("Settings", null, typeof(SettingsPage)));

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;
            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ViewMain)));
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            NavigationPage.SetHasBackButton(this, false);
            this.BindingContext = new
            {
                Header = "",
                Image = "http://www3.hilton.com/resources/media/hi/GSPSCHF/en_US/img/shared/full_page_image_gallery/main/HH_food_22_675x359_FitToBoxSmallDimension_Center.jpg",
                //Footer = "         -------- Welcome To HotelPlaza --------           "
                Footer = "Welcome To Hotel Plaza"
            };

            prefs = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterMenuItem)e.SelectedItem;
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            NavigationPage.SetHasBackButton(this, false);
            IsPresented = false;
        }
    }
}


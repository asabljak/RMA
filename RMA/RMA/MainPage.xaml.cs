using System;
using System.Collections.Generic;
using Xamarin.Forms;
using RMA.Views;

namespace RMA
{
	public partial class MainPage : MasterDetailPage
	{
        List<MasterMenuItem> MenuList { get; set; }

        public MainPage()
		{
			InitializeComponent();

            MenuList = new List<MasterMenuItem>();
            MenuList.Add(new MasterMenuItem("Settings", null, typeof(SettingsPage)));
            navigationDrawerList.ItemsSource = MenuList;

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ViewMain)));
            this.BindingContext = new
            {
                Header = "Header",
                Image = "https://rorymon.com/blog/wp-content/uploads/2014/09/Raspberry-Pi-Logo1-620x350.png",
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


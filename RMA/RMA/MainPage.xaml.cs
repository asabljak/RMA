using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using RMA.MenuItem;
using RMA.Views;

namespace RMA
{
	public partial class MainPage : MasterDetailPage
	{
        List<MasterMenuItem> menuList { get; set; }

		public MainPage()
		{
			InitializeComponent();

            menuList = new List<MasterMenuItem>();
            menuList.Add(new MasterMenuItem("Settings", null, typeof(SettingsPage)));
		}

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var item = (MasterMenuItem)e.SelectedItem;
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}


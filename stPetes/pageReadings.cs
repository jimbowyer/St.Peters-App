using System;
using Xamarin.Forms;

namespace stPetes
{
    class pageReadings : ContentPage
    {
        public pageReadings()
        {
            const string cSCRIPTUREWEB = "http://dailyscripture.servantsoftheword.org/";

            string sYear = DateTime.Now.Year.ToString();
            string sMon = DateTime.Now.ToString("MMM").ToLower();
            string sDay = DateTime.Now.Day.ToString();
            string sUrl = cSCRIPTUREWEB + "readings/" + sYear + "/" + sMon + sDay + ".htm";

            ToolbarItem tbMenu = new ToolbarItem
            {
                Text = "Mass",
                //custom menu work below for each platform
            };

            switch (Device.RuntimePlatform)
            {
                case (Device.iOS):
                    {
                        tbMenu.Order = ToolbarItemOrder.Primary;
                        ToolbarItems.Add(tbMenu);
                        tbMenu.Clicked += (sender, args) =>
                        {
                            Navigation.PushAsync(new PageReadingsMass());
                        };
                        break;
                    }
                case (Device.Android):
                    {
                        ToolbarItems.Add(new ToolbarItem("Mass", "", () =>
                        {
                            Navigation.PushAsync(new PageReadingsMass());
                        }));
                        break;
                    }
                case (Device.WinPhone):
                    {
                        tbMenu.Icon = "info.png";
                        tbMenu.Order = ToolbarItemOrder.Default;
                        ToolbarItems.Add(new ToolbarItem("Mass", "info.png", () =>
                        {
                            Navigation.PushAsync(new PageReadingsMass());
                        }));
                        break;
                    }

            } //swtich end

            WebView browser = new WebView
            {
                Source = sUrl
            };

            Content = browser;            
        }
    } //class pageReadings


}

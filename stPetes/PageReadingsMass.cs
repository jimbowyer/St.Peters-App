using System;
using Xamarin.Forms;

namespace stPetes
{
    public class PageReadingsMass : ContentPage
    {
        public PageReadingsMass()
        {            
            const string cNOVALIS = "http://novalis.rightbrainmedia.com/portals/11/Novalis/mobile.aspx?page=readings&show=1";

            WebView browser = new WebView
            {
                Source = cNOVALIS
            };

            Content = browser;
        }
    } //class PageReadingsMass
}

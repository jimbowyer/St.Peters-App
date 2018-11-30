using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace stPetes
{
    public class PageReadingsMass : ContentPage
    {
        
        public PageReadingsMass()
        {
            const string cNOVALIS = "http://ec2-34-245-7-114.eu-west-1.compute.amazonaws.com/";
            const string cREAD1 = "daily-texts/reading/";
            const string cPSALM = "daily-texts/psalm/";
            const string cREAD2 = "daily-texts/reading2/";
            const string cGOSPEL = "daily-texts/gospel/";

            string sDate = DateTime.Now.ToString("yyyy-MM-dd");
          
            //-------------------------------
            // Dictionary to hold reading Urls.
            Dictionary<string, string> readToShow = new Dictionary<string, string>
            {
                { "First Reading", (cNOVALIS + cREAD1 + sDate) },
                { "Psalms", (cNOVALIS + cPSALM + sDate) },
                { "Second Reading", (cNOVALIS + cREAD2 + sDate) },
                { "Gospel Reading", (cNOVALIS + cGOSPEL + sDate) }
            };

            WebView wvReading = new WebView{};

            Picker picker = new Picker
            {
                Title = "Press to select a reading...",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.Black,              
            };
            
            foreach (string sReadName in readToShow.Keys)
            {
                picker.Items.Add(sReadName);
            }                     

            picker.SelectedIndexChanged += (sender, args) =>
            {
                if (picker.SelectedIndex == -1)
                {
                    picker.SelectedIndex = 0;
                }
                if (readToShow.TryGetValue(picker.Items[picker.SelectedIndex], out string sUrl))
                {
                    wvReading.Source = sUrl;
                }
   
                wvReading.HeightRequest = Application.Current.MainPage.Height;
                wvReading.WidthRequest = Application.Current.MainPage.Width;
                wvReading.VerticalOptions = LayoutOptions.CenterAndExpand;
                wvReading.HorizontalOptions = LayoutOptions.CenterAndExpand;                
            };           

            // Build the page:
            Content = new StackLayout
            {
                Children =
                {
                    picker,
                    wvReading
                }
            };
        }
    } //class PageReadingsMass
}

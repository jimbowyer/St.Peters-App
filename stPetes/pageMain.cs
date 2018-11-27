using System;
using Xamarin.Forms;

namespace stPetes
{
    class pageMain : ContentPage 
    {
        Color mcolorBack = new Xamarin.Forms.Color();
        Color mcolorText = new Xamarin.Forms.Color();
        string mstrSeason;
        DayOfWeek mDoW;

        public pageMain()
        {
            GetSeasonVars(ref mstrSeason, ref mcolorBack, ref mcolorText);
            mDoW = WhatDay();

            if (Device.RuntimePlatform == Device.iOS)
            {
                mstrSeason = " " + mstrSeason + " ";
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                //set header bar color?                
               
            }


            Button btnReading = new Button
            {
                Text = mstrSeason,
                BackgroundColor = mcolorBack,
                TextColor = mcolorText,
                BorderWidth = 0,
                BorderColor = mcolorBack
            };

            btnReading.Clicked += (sender, args) =>
            {
                //open readings page:
                Navigation.PushAsync(new pageReadings());
            };

            Button btnChurchSearch = new Button
            {
                Text = "Find mass/reconciliation",
                BackgroundColor = mcolorBack,
                TextColor = mcolorText,
                BorderWidth = 0,
                BorderColor = mcolorBack
            };

            btnChurchSearch.Clicked += (sender, args) =>
            {
                //open ch search page:                
                Navigation.PushAsync(new pageChurchSearch());
            };

            Button btnBulletins = new Button
            {
                Text = "Parish Bulletins/Homilies",
                BackgroundColor = mcolorBack,
                TextColor = mcolorText,
                BorderWidth = 0,
                BorderColor = mcolorBack
            };

            btnBulletins.Clicked += (sender, args) =>
            {
                //open bulletins page:                
                Navigation.PushAsync(new pageHomilies());
            };

            Button btnMysteries = new Button
            {
                Text = TodaysMysteries(mDoW),
                BackgroundColor = mcolorBack,
                TextColor = mcolorText,
                BorderWidth = 0,
                BorderColor = mcolorBack
            };
            btnMysteries.Clicked += (sender, args) =>
            {                
                //open rosary mysteries page:                
                Navigation.PushAsync(new pageMysteries(mDoW));
            };

            Image imgBack = new Image
            {
                Source = "stPete.png",
                IsVisible = true,
                Aspect = Aspect.AspectFit
            };

            ToolbarItem tbMenu = new ToolbarItem
            {
                Text = "about",
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
                            Navigation.PushAsync(new pageAbout());
                        };
                        break;
                    }
                case (Device.Android):
                    {
                        
                        ToolbarItems.Add(new ToolbarItem("About", "info.png", () =>
                        {
                            Navigation.PushAsync(new pageAbout());
                        }));
                        break;
                    }
                case (Device.WinPhone):
                    {
                        tbMenu.Icon = "info.png";
                        tbMenu.Order = ToolbarItemOrder.Default;
                        ToolbarItems.Add(new ToolbarItem("About", "info.png", () =>
                        {
                            Navigation.PushAsync(new pageAbout());
                        }));
                        break;
                    }

            } //swtich end

            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.End,
                Padding = new Thickness(5),
                Spacing = 10,
                Children =
                    {
                     btnReading, btnChurchSearch, btnBulletins, btnMysteries
                    }               
            };

            BackgroundImage =  "stPete.png";
            
        } //constructor - pageMain


        private void GetSeasonVars(ref string pSeason, ref Color pcolorBack, ref Color pcolorText)
        {
            RomCal cal = new RomCal();
            Season seaNow;

            try
            {
                seaNow = cal.SeasonOf(DateTime.Now);
                pSeason = seaNow.SeasonName + " - " + DateTime.Now.ToString("D");

                switch (seaNow.SeasonColor)
                {
                    case Season.Colors.Green:
                        pcolorBack = Color.Green;
                        pcolorText = Color.White;
                        break;
                    case Season.Colors.Purple:
                        pcolorBack = Color.Purple;
                        pcolorText = Color.White;
                        break;
                    case Season.Colors.Red:
                        pcolorBack = Color.Red;
                        pcolorText = Color.White;
                        break;
                    case Season.Colors.White:
                        pcolorBack = Color.White;
                        pcolorText = Color.Silver;
                        break;
                    default:
                        pcolorBack = Color.Green;
                        pcolorText = Color.White;
                        break;
                }
            }
            catch (Exception)
            {
                //we errored - log later - rtn safe
                pSeason = "Undefined Season";
                pcolorBack = Color.Blue;
                pcolorText = Color.White;
            }
            
        } //GetSeasonVars

        private DayOfWeek WhatDay()
        {
            DateTime dteNow = DateTime.Now;
            return dteNow.DayOfWeek;
        }

        private string TodaysMysteries(DayOfWeek DoW)
        {            
            try
               {
                string sReturn = "";                

                switch (DoW)
                {
                    case DayOfWeek.Sunday:
                        sReturn = "Today - Glorious Mysteries";
                        break;
                    case DayOfWeek.Monday:
                        sReturn = "Today - Joyful Mysteries";
                        break;
                    case DayOfWeek.Tuesday:
                        sReturn = "Today - Sorrowful  Mysteries";
                        break;
                    case DayOfWeek.Wednesday:
                        sReturn = "Today - Glorious  Mysteries";
                        break;
                    case DayOfWeek.Thursday:
                        sReturn = "Today - Luminous  Mysteries";
                        break;
                    case DayOfWeek.Friday:
                        sReturn = "Today - Sorrowful  Mysteries";
                        break;
                    case DayOfWeek.Saturday:
                        sReturn = "Today - Joyful  Mysteries";
                        break;
                }

                return sReturn;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }//TodaysMysteries
        }
    } //class pageMain
}

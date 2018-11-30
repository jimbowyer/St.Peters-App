using System;
using Xamarin.Forms;
using System.Security;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace stPetes
{
    public class App : Application
    {
        [SecuritySafeCriticalAttribute]       
        public App()
        {
            try
            {
                // The root page of  application
                MainPage = new NavigationPage(new pageMain() { Title = " St. Peters" })
                {
                    //need white txt for androud but cant do if logic here :( White background hides android nav controls :(
                    //BarBackgroundColor = Color.White,
                    BarTextColor = Color.Silver
                   
                };
                NavigationPage.SetHasNavigationBar(this, true);

            }
            catch (Exception ex)
            {
                MainPage.DisplayAlert("app error", ex.Message, "OK");
            }
        } //App

        
        protected override void OnStart()
        {
            // Handle when your app starts... 
            // AppCenter Analytics:
            AppCenter.Start("android=c66a3e78-b655-4033-a1cc-b03418174710;" +
                  "uwp={Your UWP App secret here};" +
                  "ios=00fa113e-550e-47b9-8481-5e4e7f4a2275",
                  typeof(Analytics), typeof(Crashes));

            AppCenter.LogLevel = LogLevel.Verbose;
        } 

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
    } //class App
    
} //ns stpetes

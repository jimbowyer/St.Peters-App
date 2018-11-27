using System;
using Xamarin.Forms;
using System.Security;

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
            // Handle when your app starts... permissions:
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

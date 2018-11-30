using System;
using Xamarin.Forms;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using stPetes.Services;

namespace stPetes
{
    public class pageHomilies : ContentPage
    {
        const string cSOT_FEED_URL = "http://swordsoftruth.com/category/homilies-2/feed/";
        const string cNAMESPACE = "http://search.yahoo.com/mrss/";
        public ObservableCollection<Feed> _Feeds;
        ListView _lvHomilies = new ListView()
        {
            ItemTemplate = new DataTemplate(typeof(FeedTemplate)),
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            HasUnevenRows = false
        };
        Label _lblHeader = new Label()
        {
            HorizontalTextAlignment = TextAlignment.Center,
            Text = "Homilies"
        };
        Label lblBulletins = new Label()
        {
            HorizontalTextAlignment = TextAlignment.Center,
            Text = "~ Parish Bulletins ~",
            FontSize = 15
        };
        Button btnBulletinCurrent = new Button
        {
            Text = "Current Week's",
            BorderWidth = 0,
        };
        Button btnBulletinLast = new Button
        {
            Text = "Last Week's",
            BorderWidth = 0,
        };

    public pageHomilies()
        {
            XDocument rssDoc = new XDocument();

            LoadFeeds();

            _lvHomilies.ItemTapped += async (sender, e) =>
            {
                pageHomily pgHomily = new pageHomily(e);
                await _lvHomilies.Navigation.PushAsync(pgHomily);
            };

            _lvHomilies.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };

            btnBulletinCurrent.Clicked += (sender, args) =>
            {
                try
                {
                    Uri uBull = new Uri(GetBulletinUrl(DateTime.Now));                    

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.UWP)
                    {
                        Device.OpenUri(uBull);                    
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        LoadPDF(this.btnBulletinCurrent, uBull);                        
                    }
                }
                catch(Exception ex)
                {
                    DisplayAlert("Error!", "Sorry, error loading bulletin: " + ex.Message, "cancel");
                }
               
            }; //btnBulletinCurrent.Clicked

            btnBulletinLast.Clicked += (sender, args) =>
            {                
                try
                {
                    Uri uBull = new Uri(GetBulletinUrl(DateTime.Now.Subtract(TimeSpan.FromDays(7))));

                    if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.UWP)
                    {
                        Device.OpenUri(uBull);
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        LoadPDF(this.btnBulletinLast, uBull);                        
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error!", "Sorry, error loading bulletin: " + ex.Message, "cancel");
                }
            }; // btnBulletinLast.Clicked

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                        _lblHeader,
                        _lvHomilies,
                        lblBulletins,
                        new StackLayout {
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.Center,
						Children = { btnBulletinLast, btnBulletinCurrent }
					}                        
                }
            };

        } //pageHomilies constructor

        private async void LoadPDF(Button sender, Uri uBulletin)
        {
            try
            {                
                Ipdf pdfWorker = DependencyService.Get<Ipdf>();
                Task<int> tskPdf = pdfWorker.OpenPdf(uBulletin.ToString());
                string sText = sender.Text;
                sender.IsEnabled = false;
                sender.Text = "downloading...";
                int x = await tskPdf;
                sender.IsEnabled = true;
                sender.Text = sText;
            }            
            catch (Exception ex)
            {
                //Debug.Write(ex.Message);
                await DisplayAlert("Error loading pdf", ex.Message, "ok");
            }
        } //LoadPDF

        private async void LoadFeeds()
        {
            _lblHeader.Text = "loading...";
            string sResult = await DownloadFeeds();
            //set label & bind itemsource: 
            _lblHeader.Text = sResult;
            _lvHomilies.ItemsSource = _Feeds;
        }

        private string GetBulletinUrl(DateTime dte2Get)
        {
            StringBuilder sb2Return = new StringBuilder("https://st-peters.ca/wp-content/uploads/");
            //#1: get bulletin date for current or last sunday (published on saturday):
            DateTime dteBulletin = dte2Get;
            while (dteBulletin.DayOfWeek != DayOfWeek.Saturday)
            {
                dteBulletin = dteBulletin.Subtract(TimeSpan.FromDays(1));
            }
            dteBulletin = dteBulletin.Add(TimeSpan.FromDays(1)); 

            //#2: Build bulletin url - month must be uppercase so need extra format:
            sb2Return.Append(dteBulletin.Date.ToString("dd"));
            sb2Return.Append("-");
            sb2Return.Append(dteBulletin.Date.ToString("MMM").ToUpper());
            sb2Return.Append("-");
            sb2Return.Append(dteBulletin.Date.ToString("yy"));
            sb2Return.Append(".pdf");

            return sb2Return.ToString();

        } //GetBulletinUrl

        public async Task<string> DownloadFeeds()
        {
            try
            {
                var httpClient = new HttpClient(); //uses nuget microsoft.net.http

                Task<string> feedsTask = httpClient.GetStringAsync(cSOT_FEED_URL); // async method!            
                string sFeeds = await feedsTask;
                XDocument doc = XDocument.Parse(sFeeds);
                XNamespace nsMedia = cNAMESPACE;
                List<Feed> Feeds = new List<Feed>();
                
                foreach (XElement item in doc.Element("rss").Element("channel").Elements("item"))
                {
                    try
                    {
                        Feed feed2Add = new Feed();
                        feed2Add.Title = item.Element("title").Value;
                        feed2Add.Link = item.Element("link").Value;
                        feed2Add.Description = item.Element("description").Value;
                        feed2Add.PublicationDate = DateTime.Parse(item.Element("pubDate").Value);
                        feed2Add.GUID = item.Element("guid").Value;
                        feed2Add.Media = item.Elements("enclosure").ElementAt(0).Attribute("url").Value;
                        feed2Add.ThumbNail = item.Element(nsMedia + "thumbnail").Attribute("url").Value;
                        Feeds.Add(feed2Add);
                    }
                    catch
                    {
                        //skip over feed!
                        //just in case any feed has bad formed XML we can ignore and still share other homilies
                        //e.g. missing thumbnail elements!
                    }
                }

                _Feeds = new ObservableCollection<Feed>(Feeds);

                string exampleInt = Feeds.Count.ToString() + " homilies loaded:";
                return exampleInt;
            }
            catch (Exception ex)
            {
                return "Sorry, error loading homilies: " + ex.Message;
            }
        }
    } //class PageHomilies

    public class Feed
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string GUID { get; set; }
        public string Media { get; set; }
        public string ThumbNail { get; set; }
    }

    public class FeedTemplate : ViewCell
    {
        public FeedTemplate() : base()
        {            
        }
        protected override void OnBindingContextChanged()
        {            
            base.OnBindingContextChanged();

            var lblTitle = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 18
            };

            lblTitle.SetBinding(Label.TextProperty, "Title");
            this.View = lblTitle;
            this.Height = this.View.HeightRequest;

            this.View.BindingContext = this.BindingContext;
        }
    } //FeedTemplate class    
}

using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using stPetes.Services;


namespace stPetes
{
    public class pageHomily : ContentPage
    {
        public ProgressBar _showPlayed;
        public Label _lblProgress;
        public pageHomily(Xamarin.Forms.ItemTappedEventArgs oFeed)
        {
            Image webImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 200 };
            _showPlayed = new ProgressBar { HeightRequest = 50, Progress = 0 };
            _lblProgress = new Label { FontSize = 10 };
            string sLink = "not set";

            Feed feedToPlay = oFeed.Item as Feed;
            sLink = feedToPlay.Media;
            try
            {
                webImage.Source = ImageSource.FromUri(new Uri(feedToPlay.ThumbNail));               
            }
            catch
            {
                webImage.Source = ImageSource.FromFile("about.jpg");
            }

            Label lblTitle = new Label
            {
                Text = feedToPlay.Title + ". " + feedToPlay.PublicationDate.ToString(),
                FontSize = 15

            };
            Button btnStop = new Button
            {
                Text = "Stop",
                HorizontalOptions = LayoutOptions.Center,
                Command = new Command(() =>
                {
                    DependencyService.Get<IAudio>().StopAudio();
                })
            };
            Button btnPause = new Button
            {
                Text = "Pause",
                HorizontalOptions = LayoutOptions.Center,
                Command = new Command(() =>
                {
                    DependencyService.Get<IAudio>().PauseAudio();
                })
            };
            Button btnPlay = new Button
            {
                Text = "Play-Homily-Audio",
                HorizontalOptions = LayoutOptions.Center,
                Command = new Command(() =>
                {
                    StartPlayHandler(sLink, null);
                })
            };

            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(5),
                Spacing = 20,

                Children =
                {
                    lblTitle,
                    webImage,
                    _showPlayed,
                    _lblProgress,
                    new StackLayout
                    {
                        Spacing = 15,
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            btnStop,
                            btnPause,
                            btnPlay
                        }
                    }
                }
            };

        } //contruct

        private async void StartPlayHandler(object sender, System.EventArgs e)
        {
            //handle event call to update progress bar and time played label
            TimeSpan t;
            TimeSpan d;
            _showPlayed.Progress = 0;
            Progress<PlayProgress> progressReporter = new Progress<PlayProgress>();
            progressReporter.ProgressChanged += (s, args) =>
            {
                _showPlayed.ProgressTo(args.PercentComplete, 100, Easing.Linear);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    t = TimeSpan.FromSeconds(args.CurrentPosition);
                    d = TimeSpan.FromSeconds(args.Duration);
                }
                else //default
                {
                    t = TimeSpan.FromMilliseconds(args.CurrentPosition);
                    d = TimeSpan.FromMilliseconds(args.Duration);
                }
                _lblProgress.Text = t.ToString(@"hh\:mm\:ss") + " / " + d.ToString(@"hh\:mm\:ss");
            };
            string sLink = (string)sender;
            Task<int> PlayTask = DependencyService.Get<IAudio>().StartPlayTask(sLink, progressReporter);
            int iCurrentPos = await PlayTask;
            Debug.WriteLine("*** Player position is {0} secs ***", iCurrentPos);

        } //StartPlayHandler

        protected override void OnDisappearing()
        {
            DependencyService.Get<IAudio>().StopAudio();
        }

    } //class
}//ns

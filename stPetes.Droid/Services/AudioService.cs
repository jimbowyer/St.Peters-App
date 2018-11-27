using Xamarin.Forms;
using Android.Media;
using System.Threading.Tasks;
using System.Threading;
using System;
using stPetes.Services;
using stPetes.Droid.Services;

[assembly: Dependency(typeof(AudioService))]

namespace stPetes.Droid.Services
{
    public class AudioService : IAudio
    {
        public AudioService() { }

        private MediaPlayer _mediaPlayer;
        private string _fileName;

        public async Task<int> StartPlayTask(string fileName, IProgress<PlayProgress> progessReporter)
        {
            int iCurrentPosition = 0;

            Android.Net.Uri uri2Play = Android.Net.Uri.Parse(fileName);

            if (_mediaPlayer != null) //already loaded so restart if paused
            {
                if (_fileName == fileName)  //check we havent change tracks
                {
                    _mediaPlayer.Start();
                }
                else
                {
                    //track change, reset player:
                    _fileName = fileName;
                    _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, uri2Play);
                    _mediaPlayer.Start();
                }
            }
            else
            {
                //new player:             
                _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, uri2Play);
                _mediaPlayer.Start();
            }
            while (_mediaPlayer.IsPlaying) //update play progress
            {
                if (progessReporter != null)
                {
                    iCurrentPosition = _mediaPlayer.CurrentPosition;
                    PlayProgress args = new PlayProgress(fileName, iCurrentPosition, _mediaPlayer.Duration);
                    progessReporter.Report(args);
                    await Task.Yield();
                }
            }
            return iCurrentPosition;

        }

        public void PauseAudio()
        {
            if (_mediaPlayer != null && _mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
            }
        }

        public void StopAudio()
        {
            if (_mediaPlayer != null && _mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Stop();
            }
        }

    } //class
}
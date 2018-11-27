using stPetes.Services;
using Xamarin.Forms;
using AVFoundation;
using Foundation;
using System;
using System.Threading.Tasks;
using stPetes.iOS.Services;

[assembly: Dependency(typeof(AudioService))]

namespace stPetes.iOS.Services
{
    class AudioService : NSObject, IAudio
    {
        public AudioService() { }

        protected AVAudioPlayer _mediaPlayer;
        private string _fileName;


        public async Task<int> StartPlayTask(string fileName, IProgress<PlayProgress> progessReporter)
        {
            int iCurrentPosition = 0;
            NSUrl url;
            NSData data;
            NSError err;

            if (_mediaPlayer != null) //already loaded so restart if paused
            {
                if (_fileName == fileName)  //check we havent change tracks
                {
                    _mediaPlayer.Play();
                }
                else
                {
                    //track change, reset player:
                    _fileName = fileName;
                    url = NSUrl.FromString(fileName);
                    data = NSData.FromUrl(url);
                    err = null;

                    _mediaPlayer = AVAudioPlayer.FromData(data, out err);
                    if (_mediaPlayer != null)
                    {
                        _mediaPlayer.Play();
                    }
                }
            }
            else
            {
                //new player:             
                _fileName = fileName;
                url = NSUrl.FromString(fileName);
                data = NSData.FromUrl(url);
                err = null;

                _mediaPlayer = AVAudioPlayer.FromData(data, out err);
                if (_mediaPlayer != null)
                {
                    _mediaPlayer.Play();
                }
            }
            while (_mediaPlayer.Playing) //update play progress
            {
                if (progessReporter != null)
                {
                    iCurrentPosition = (int)_mediaPlayer.CurrentTime;
                    PlayProgress args = new PlayProgress(fileName, iCurrentPosition, (int)_mediaPlayer.Duration);
                    progessReporter.Report(args);
                    await Task.Yield();
                }
            }
            return iCurrentPosition;
        } //StartPlayTask

        public void PauseAudio()
        {
            if (_mediaPlayer != null && _mediaPlayer.Playing)
            {
                _mediaPlayer.Pause();
            }
        }

        public void StopAudio()
        {
            if (_mediaPlayer != null && _mediaPlayer.Playing)
            {
                _mediaPlayer.Stop();
            }
        }

    } //class
}//ns

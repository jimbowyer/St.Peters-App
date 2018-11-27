using System;
using System.Threading.Tasks;

namespace stPetes.Services
{
    public interface IAudio
    {
        Task<int> StartPlayTask(string fileName, IProgress<PlayProgress> progessReporter);
        void StopAudio();
        void PauseAudio();
    }
}

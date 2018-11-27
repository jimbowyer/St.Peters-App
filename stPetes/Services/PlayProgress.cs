
namespace stPetes.Services
{
    public class PlayProgress
    {
        public PlayProgress(string fileName, int currentPosition, int duration)
        {
            Filename = fileName; //do we care?
            CurrentPosition = currentPosition;
            Duration = duration;
        }

        public int Duration { get; private set; }

        public int CurrentPosition { get; private set; }

        public float PercentComplete { get { return (float)CurrentPosition / Duration; } }

        public string Filename { get; private set; }

        public bool IsFinished { get { return CurrentPosition == Duration; } }
    } //class
}

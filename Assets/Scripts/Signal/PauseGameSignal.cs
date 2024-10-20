using Tools;

namespace Tatedrez.Core
{
    public class PauseGameSignal : SignalData
    {
        public readonly bool IsPaused;

        public PauseGameSignal(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}

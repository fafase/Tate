using Tools;

namespace Tatedrez.Core
{
    public class PawnMovementSignal : SignalData
    {
        public readonly bool StartMovement;

        public PawnMovementSignal(bool startMovement)
        {
            StartMovement = startMovement;
        }
    }
}

using Tools;
namespace Tatedrez.Core
{
    public class PawnMovementSignal : SignalData
    {
        public readonly bool StartMovement;
        public readonly IPawn Pawn;
        public readonly ITile Tile;

        public PawnMovementSignal(bool startMovement, IPawn pawn, ITile tile)
        {
            StartMovement = startMovement;
            Pawn = pawn;
            Tile = tile;
        }
    }
}

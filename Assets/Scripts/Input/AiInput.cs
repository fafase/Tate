using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tatedrez.Core;
using Zenject;

public class AiInput : InputBase
{
    [Inject] private IMovementService m_movementService;
    [Inject] private ICoreController m_core;
    [Inject] private IGrid m_grid;

    private List<IPawn> m_pawns;
    private CancellationTokenSource m_source = new();
    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {   
        if(m_pawns == null) 
        {
            m_pawns = m_core.Pawns.FindAll(pawn => pawn.PawnTurn.Equals(Turn.Player2));
        }
        AI_Click().Forget();   
    }
    public override void StopInput()
    {
        base.StopInput();
        m_source.Cancel();
    }
    private async UniTask AI_Click() 
    {
        await UniTask.Yield();
        if (m_source.IsCancellationRequested) { return; }
        List<(int row, int col)> moves = new();
        IPawn selectedPawn = null;
        if (!m_core.AllPawnsOnDeck)
        {
            selectedPawn = m_pawns.First(pawn => !pawn.HasMovedToDeck);         
        }
        else
        {
            List<IPawn> pawns = new();
            foreach (IPawn pawn in m_pawns)
            {
                if (m_movementService.CheckForAllowedMoves(pawn).Count > 0) 
                {
                    pawns.Add(pawn);
                }
            }
            selectedPawn = pawns[UnityEngine.Random.Range(0, pawns.Count - 1)];
        }
        m_core.SetSelectedPawn(selectedPawn);
        moves.AddRange(m_movementService.CheckForAllowedMoves(selectedPawn));
        var (row, col) = moves[UnityEngine.Random.Range(0, moves.Count - 1)];
        (m_core.Tiles[row, col] as IClickable).OnPress();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreController : MonoBehaviour, ICoreController
{
    [SerializeField] private GameObject m_pawnContainer;
    [SerializeField] private GameObject m_tileContainer;

    public IPawn SelectedPawn { get; private set; }

    public IReactiveProperty<Turn> CurrentTurn { get; private set; } = new ReactiveProperty<Turn>(Turn.Player1);
    public IReactiveProperty<Movement> LastMovement { get; private set; } = new ReactiveProperty<Movement>();

    private List<IPawn> m_pawns = new List<IPawn>();

    public bool AllPawnsOnDeck => m_pawns.All(p => p.HasMovedToDeck);
    private GridController m_game;

    void Start() 
    {
        m_pawns = m_pawnContainer.GetComponentsInChildren<IPawn>(true).ToList();
        m_game = new GridController();
        m_game.Init(this);
    }

    void OnDestroy() 
    {
        (CurrentTurn as IDisposable)?.Dispose();
    }

    public void SetSelectedPawn(IPawn pawn) 
    {
        SelectedPawn = pawn;
    }

    public void MoveSelectedToPosition(ITile tile) 
    {
        if(SelectedPawn == null) { return; }
        SelectedPawn.MoveToPosition(tile);
        CurrentTurn.Value = CurrentTurn.Value == Turn.Player1 ? Turn.Player2 : Turn.Player1;
        LastMovement.Value = new Movement(SelectedPawn, tile);
        SelectedPawn = null;
    }

    public void SetWin(Turn pawnTurn) 
    {
        Debug.LogWarning($"{pawnTurn} won");
    }
}
public struct Movement 
{
    public readonly IPawn pawn;
    public readonly ITile tile;

    public Movement(IPawn pawn, ITile tile)
    {
        this.pawn = pawn;
        this.tile = tile;
    }
}
public enum Turn { Player1, Player2 }
public interface ICoreController
{
    IPawn SelectedPawn { get; }
    void SetSelectedPawn(IPawn pawn);
    void MoveSelectedToPosition(ITile tile);
    void SetWin(Turn pawnTurn);
    IReactiveProperty<Turn> CurrentTurn { get; }
    IReactiveProperty<Movement> LastMovement { get; }
    bool AllPawnsOnDeck { get;  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject.SpaceFighter;

public class CoreController : MonoBehaviour, ICoreController
{
    [SerializeField] private GameObject m_pawnContainer;
    private Turn m_current;
    public IPawn SelectedPawn { get; private set; }

    public IReactiveProperty<Turn> CurrentTurn { get; private set; } = new ReactiveProperty<Turn>(Turn.Player1);

    private List<IPawn> m_pawns = new List<IPawn>();

    public bool AllPawnsOnDeck => m_pawns.All(p => p.HasMovedToDeck);

    void Start() 
    {
        m_pawns = m_pawnContainer.GetComponentsInChildren<IPawn>(true).ToList();
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
        SelectedPawn = null;
        CurrentTurn.Value = CurrentTurn.Value == Turn.Player1 ? Turn.Player2 : Turn.Player1;
    }
}

public enum Turn { Player1, Player2 }
public interface ICoreController
{
    IPawn SelectedPawn { get; }
    void SetSelectedPawn(IPawn pawn);
    void MoveSelectedToPosition(ITile tile);
    IReactiveProperty<Turn> CurrentTurn { get; }
    bool AllPawnsOnDeck { get;  }
}
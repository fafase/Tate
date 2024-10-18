using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tatedrez.Core
{
    public class CoreController : MonoBehaviour, ICoreController
    {
        [SerializeField] private GameObject m_pawnContainer;
        [SerializeField] private GameObject m_tileContainer;

        public IPawn SelectedPawn { get; private set; }

        public IReactiveProperty<Turn> CurrentTurn { get; private set; } = new ReactiveProperty<Turn>(Turn.Player1);
        public IReactiveProperty<Movement> LastMovement { get; private set; } = new ReactiveProperty<Movement>();

        private List<IPawn> m_pawns = new List<IPawn>();

        public bool AllPawnsOnDeck => m_pawns.All(p => p.HasMovedToDeck);

        private GridController m_grid;
        private MovementController m_movement;
        void Start()
        {
            m_pawns = m_pawnContainer.GetComponentsInChildren<IPawn>(true).ToList();
            m_grid = new GridController(this);
            m_movement = new MovementController(m_grid);
        }

        void OnDestroy()
        {
            (CurrentTurn as IDisposable)?.Dispose();
        }

        public void SetSelectedPawn(IPawn pawn)
        {
            SelectedPawn = pawn;
            m_movement.CheckForMovement(pawn);
        }

        public void MoveSelectedToPosition(ITile tile)
        {
            if (SelectedPawn == null) { return; }
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
        bool AllPawnsOnDeck { get; }
    }
}
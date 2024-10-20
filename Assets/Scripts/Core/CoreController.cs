using Rx;
using System;
using System.Collections.Generic;
using System.Linq;
using Tatedrez.UI;
using Tools;
using UnityEngine;
using Zenject;
using Tatedrez.Core;

namespace Tatedrez.Core
{
    public class CoreController : MonoBehaviour, ICoreController
    {
        [Inject] private IMovementService m_movementService;
        [Inject] private IPopupManager m_popupManager;

        [SerializeField] private GameObject m_pawnContainer;
        [SerializeField] private GameObject m_tileContainer;
        private CompositeDisposable m_compositeDisposable = new CompositeDisposable();
        public IPawn SelectedPawn { get; private set; }

        public IReactiveProperty<Turn> CurrentTurn { get; private set; } = new ReactiveProperty<Turn>(Turn.Player1);
        public IReactiveProperty<Movement> LastMovement { get; private set; } = new ReactiveProperty<Movement>();

        private List<IPawn> m_pawns = new List<IPawn>();
        
        public bool AllPawnsOnDeck => m_pawns.All(p => p.HasMovedToDeck);
        private EndGameService m_endGameService;

        void Start()
        {
            m_pawns = m_pawnContainer.GetComponentsInChildren<IPawn>(true).ToList();
        }

        void OnDestroy()
        {
            (CurrentTurn as IDisposable)?.Dispose();
            m_compositeDisposable?.Dispose();
            m_endGameService?.Dispose();
        }

        public void SetSelectedPawn(IPawn pawn)
        {
            SelectedPawn = pawn;
            if (AllPawnsOnDeck)
            {
                m_movementService.SetPotentialTiles(pawn);
            }
        }

        public void MoveSelectedToPosition(ITile tile)
        {
            if (SelectedPawn == null) { return; }
            Signal.Send(new PawnMovementSignal(true));
            m_movementService.ResetTiles();

            IDisposable disposable = null;
            disposable = SelectedPawn
                .MoveToPosition(tile)
                .Subscribe(new Observer<Unit>(onNext: _ => { },
                onCompleted: () =>
                {
                    LastMovement.Value = new Movement(SelectedPawn, tile);
                    var nextTurn = CurrentTurn.Value == Turn.Player1 ? Turn.Player2 : Turn.Player1;
                    if (!AllPawnsOnDeck || m_movementService.HasPotentialMove(m_pawns, nextTurn))
                    {
                        CurrentTurn.Value = nextTurn;
                    }
                    disposable.Dispose();
                    SelectedPawn = null;
                    Signal.Send(new PawnMovementSignal(false));
                }))
                .AddTo(m_compositeDisposable);
        }

        public void SetWin(Turn pawnTurn)
        {
            Signal.Send(new EndGameSignal());
            m_endGameService = new EndGameService();
            m_endGameService.
                EndLevelSequence(m_pawns, pawnTurn)
                .Subscribe(_ => { }, () => 
                {
                    EndLevelPopup popup = (EndLevelPopup)m_popupManager.Show<EndLevelPopup>();
                    popup.InitWithWinner(pawnTurn);
                    popup.OnClose.Subscribe(_ => m_endGameService.Dispose())
                    .AddTo(m_compositeDisposable);
                });
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
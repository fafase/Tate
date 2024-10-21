using Rx;
using System;
using System.Collections;
using Tools;
using UnityEngine;
using Tatedrez.Input;
using Zenject;

namespace Tatedrez.Core
{
    public class PawnInputController : MonoBehaviour, IClickable, IPawn
    {
        [Inject] private ICoreController m_core;
        [SerializeField] private Turn m_item;
        [SerializeField] private GameObject m_background;
        [SerializeField] private PawnType m_pawnType;

        [Header("Animation values")]
        [SerializeField] private float m_period = 0.5f;
        [SerializeField] private float m_maxScale = 1.5f;

        private static PawnInputController m_selected;
        private Turn m_current;
        public ITile CurrentTile { get; private set; }
        public bool HasMovedToDeck { get; private set; }
        public PawnType PawnType => m_pawnType;

        public Turn Owner => m_item;
        public Transform Transform => transform;
        private SpriteRenderer m_spriteRenderer;

        private const int s_orderLayerMove = 10;
        private const int s_orderLayerIdle = 5;

        void Start()
        {
            m_core
                .CurrentTurn
                .Subscribe(turn => m_current = turn);
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnPress()
        {
            if (m_item != m_current)
            {
                return;
            }
            if (HasMovedToDeck && !m_core.AllPawnsOnDeck)
            {
                return;
            }
            if (m_selected != null && m_selected != this)
            {
                m_selected.SetBackground(false);
            }
            Signal.Send(new AudioSignal(AudioSignal.Tap));
            m_selected = this;
            m_core.SetSelectedPawn(this);
            SetBackground(!m_background.activeSelf);
        }


        private void SetBackground(bool value)
        {
            m_background.SetActive(value);
        }

        public IObservable<Unit> MoveToPosition(ITile tile)
        {
            return Observable.Create<Unit>(observer => 
            {
                Signal.Send(new AudioSignal(AudioSignal.Bloop));
                m_spriteRenderer.sortingOrder = s_orderLayerMove;
                HasMovedToDeck = true;
                CurrentTile?.FreeTile();
                CurrentTile = tile;
                Vector3 position = CurrentTile.Position;
                position.z = -1f;
                StartCoroutine(LerpSequence(observer, position, 0.25f));
                
                SetBackground(false);
                return Disposable.Create(() => 
                {
                    m_spriteRenderer.sortingOrder = s_orderLayerIdle;
                });
            });
        }


        private IEnumerator LerpSequence(IObserver<Unit> observable, Vector3 target, float period)
        {
            float ratio = 0f;

            Vector3 start = transform.position;
            Vector3 scale = transform.localScale;
            Vector3 maxScale = scale * m_maxScale;
            while (ratio < 1f)
            {
                ratio += Time.deltaTime / m_period;
                transform.position = Vector3.Lerp(start, target, ratio);

                float scaleRatio = Mathf.Sin(ratio * Mathf.PI); 
                transform.localScale = Vector3.Lerp(scale, maxScale, scaleRatio);

                observable.OnNext(Unit.Default);
                yield return null;
            }
            transform.position = target;
            observable.OnCompleted();
        } 
    }

    public enum PawnType
    {
        Horse, Tower, Bishop
    }
    public interface IPawn
    {
        IObservable<Unit> MoveToPosition(ITile tile);
        ITile CurrentTile { get; }
        bool HasMovedToDeck { get; }
        Turn Owner { get; }
        PawnType PawnType { get; }
        Transform Transform { get; }
    }
}
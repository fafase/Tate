using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using State = Tools.IPopup.State;
using Rx;

namespace Tools
{
    [RequireComponent(typeof(Animation))]
    public abstract class Popup : MonoBehaviour, IPopup
    {
        [SerializeField] protected Button m_closeBtn;
        [SerializeField] private AnimationClip m_openAnimation;
        [SerializeField] private AnimationClip m_closeAnimation;

        private IPopupManager m_popupManager;
        private Animation m_animation;
        private State m_state = State.Idle;
        public State PopupState => m_state;

        public ISubject<Unit> OnClose { get; private set; } =  new Subject<Unit>();
        public ISubject<Unit> OnOpen { get; private set; } = new Subject<Unit>();
        public bool IsOpen => m_state == State.Opening || m_state == State.Idle;
        protected CompositeDisposable m_compositeDisposable = new CompositeDisposable();

        public virtual void Init(IPopupManager popupManager)
        {
            m_popupManager = popupManager;
            transform.SetParent(m_popupManager.Container, false);
            if (m_closeBtn != null)
            {
                m_closeBtn.onClick.AddListener(() => Close());
            }
            m_animation = GetComponent<Animation>();
            m_animation.wrapMode = WrapMode.Once;
            AddAnimation(m_openAnimation);
            AddAnimation(m_closeAnimation);

            m_state = State.Idle;
            Signal.Send(new AudioSignal(AudioSignal.Swoosh));
            StartCoroutine(OpenSequence());
        }

        protected virtual void OnDestroy() 
        {
            m_compositeDisposable?.Dispose();
        }

        private void AddAnimation(AnimationClip clip) 
        {
            if(clip != null) 
            {
                m_animation.AddClip(clip, clip.name);
            }          
        }

        private IEnumerator OpenSequence() 
        {
            yield return StartCoroutine(AnimationSequence(State.Opening));
            SetButtons(true);
            OnOpen.OnNext(Unit.Default);
        }

        private IEnumerator CloseSequence() 
        {
            yield return StartCoroutine(AnimationSequence(State.Closing));
            OnClose.OnNext(Unit.Default);
            Destroy(gameObject);
        }


        private IEnumerator AnimationSequence(State state) 
        {
            m_state = state;
            SetButtons(false);
            AnimationClip currentAnim = state == State.Opening ? m_openAnimation : m_closeAnimation;
            m_animation.clip = currentAnim;
            m_animation.Play();
            while (m_animation.IsPlaying(currentAnim.name))
            {
                yield return null;
            }
            m_state = State.Idle;
        }

        public void Close(bool closeImmediate = false) 
        {
            Signal.Send(new AudioSignal(AudioSignal.Swoosh));
            m_popupManager.Close(this);
            if (closeImmediate) 
            {
                Destroy(gameObject);
                return;
            }
            StartCoroutine(CloseSequence());
        }

        private void SetButtons(bool value) => Array.ForEach(GetComponentsInChildren<Button>(), (btn => btn.enabled = value));

        public class Factory : PlaceholderFactory<Popup, Popup> { }
    }

    public interface IPopup 
    {
        void Close(bool closeImmediate = false);
        void Init(IPopupManager popupManager);
        ISubject<Unit> OnOpen { get; }
        ISubject<Unit> OnClose { get; }

        State PopupState { get; }
        bool IsOpen { get; }
        public enum State
        {
            Idle, Opening, Closing
        }
    }
}

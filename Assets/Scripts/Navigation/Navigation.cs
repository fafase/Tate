using Rx;
using Tatedrez.Core;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class Navigation : MonoBehaviour
{
    [SerializeField] protected Button m_loadButton;
    [Inject] protected IPopupManager m_popupManager;
    private CompositeDisposable m_compositeDisposable = new CompositeDisposable();

    protected virtual void Start()
    {
        m_loadButton.onClick.AddListener(() => OnPress());
    }

    protected virtual void OnDestroy()
    {
        m_compositeDisposable?.Dispose();
    }

    private void OnPress()
    {
        m_loadButton.interactable = false;
        Signal.Send(new PauseGameSignal(true));
        OpenPopup()
            .OnClose
            .Subscribe(_=> 
            { 
                m_loadButton.interactable = true;
                Signal.Send(new PauseGameSignal(false));
            })
            .AddTo(m_compositeDisposable);
    }

    protected abstract IPopup OpenPopup();
}

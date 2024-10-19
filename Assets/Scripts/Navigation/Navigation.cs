using Rx;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class Navigation : MonoBehaviour
{
    [SerializeField] protected Button m_loadButton;
    [Inject] protected IPopupManager m_popupManager;
    private CompositeDisposable m_compositeDisposable = new CompositeDisposable();

    private void Start()
    {
        m_loadButton.onClick.AddListener(() => OnPress());
    }

    private void OnDestroy()
    {
        m_compositeDisposable?.Dispose();
    }

    private void OnPress()
    {
        m_loadButton.interactable = false;

        OpenPopup()
            .OnClose
            .Subscribe(_=> m_loadButton.interactable=true)
            .AddTo(m_compositeDisposable);
    }

    protected abstract IPopup OpenPopup();
}

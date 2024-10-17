using System.Collections;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class Navigation : MonoBehaviour
{
    [SerializeField] protected Button m_loadButton;
    [Inject] protected IPopupManager m_popupManager;

    private void Start()
    {
        m_loadButton.onClick.AddListener(() => OnPress());
    }

    private void OnPress()
    {
        m_loadButton.interactable = false;
        IPopup popup = OpenPopup();
        popup.AddToClose(() =>
        {
            m_loadButton.interactable = true;
        });
    }

    protected abstract IPopup OpenPopup();
}

using Tools;
using Tatedrez.UI;
using UnityEngine;
using Rx;
using Zenject;
using UnityEngine.UI;

namespace Tatedrez.Meta
{
    public class MetaNavigation : MonoBehaviour
    {
        [SerializeField] protected Button m_onePlayer;
        [SerializeField] protected Button m_twoPlayer;

        [Inject] protected IPopupManager m_popupManager;
        private CompositeDisposable m_compositeDisposable = new CompositeDisposable();

        private void Start()
        {
            m_onePlayer
                .OnClickAsObservable()
                .Subscribe(_ => OnPress(Player.Single))
                .AddTo(m_compositeDisposable);

            m_twoPlayer
                .OnClickAsObservable()
                .Subscribe(_ => OnPress(Player.Two))
                .AddTo(m_compositeDisposable);
        }

        private void OnDestroy()
        {
            m_compositeDisposable?.Dispose();
        }
        private void OnPress(string player) 
        {
            PlayerPrefs.SetString(Player.Key, player);
            m_popupManager.Show<PlayLevelPopup>();
        }
    }
}

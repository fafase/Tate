using Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Rx;
using Tate.Loading;

namespace Tatedrez.UI
{
    public class LoadNextScenePopup : Popup
    {
        [SerializeField] private Button m_playButton;
        [SerializeField] private Scene m_scene;

        [Inject] private ISceneLoading m_sceneLoading;


        private void Start()
        {
            m_playButton
                .OnClickAsObservable()
                .Subscribe(_ => Load())
                .AddTo(m_compositeDisposable);
        }

        private void Load()
        {
            OnClose
                .Subscribe(_ => 
                    m_sceneLoading.LoadScene(m_scene.ToString()))
                .AddTo(m_compositeDisposable);
            Close();
        }

        enum Scene { Meta, Core }
    }
}

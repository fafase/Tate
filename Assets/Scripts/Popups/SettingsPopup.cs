using Cysharp.Threading.Tasks;
using Rx;
using System;
using Tate.Loading;
using Tatedrez.Core;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tatedrez.UI
{
    public class SettingsPopup : Popup
    {
        [SerializeField] private Button m_continueBtn;
        [SerializeField] private Button m_restartBtn;

        [Inject] private ISceneLoading m_sceneLoader;

        void Start()
        {
            m_restartBtn
                .OnClickAsObservable()
                .Subscribe(_ => HandleButtonClick(() => m_sceneLoader.LoadCore()))
                .AddTo(m_compositeDisposable);

            m_closeBtn
                .OnClickAsObservable()
                .Subscribe(_ => HandleButtonClick(() => m_sceneLoader.LoadMeta()))
                .AddTo(m_compositeDisposable);

            m_continueBtn
                .OnClickAsObservable()
                .Subscribe(_=> OnContinuePress())
                .AddTo(m_compositeDisposable);
        }

        private void HandleButtonClick(Func<UniTask> loadMethod)
        {
            Close();
            OnClose
                .Subscribe(async _ => await loadMethod())
                .AddTo(m_compositeDisposable);
        }

        private void OnContinuePress() 
        {
            Close();
        }
    }
}

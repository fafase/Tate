using UnityEngine;
using Tools;
using UnityEngine.UI;
using Rx;
using Tatedrez.Core;
using System;
using TMPro;
using Zenject;
using Cysharp.Threading.Tasks;

namespace Tatedrez.UI
{
    public class EndLevelPopup : Popup
    {
        [Inject] ISceneLoading m_sceneLoader;

        [SerializeField] private Button m_reloadBtn;
        [SerializeField] private TextMeshProUGUI m_winnerText;

        private void Start()
        {
            m_reloadBtn
                .OnClickAsObservable()
                .Subscribe(_ => HandleButtonClick(() => m_sceneLoader.LoadCore()))
                .AddTo(m_compositeDisposable);

            m_closeBtn
                .OnClickAsObservable()
                .Subscribe(_ => HandleButtonClick(() => m_sceneLoader.LoadMeta()))
                .AddTo(m_compositeDisposable);
        }

        private void HandleButtonClick(Func<UniTask> loadMethod)
        {
            Close();
            OnClose
                .Subscribe(async _ => await loadMethod())
                .AddTo(m_compositeDisposable);
        }


        public void InitWithWinner(Turn pawnTurn)
        {
            m_winnerText.text = String.Format(m_winnerText.text, pawnTurn.ToString());
        }
    }
}

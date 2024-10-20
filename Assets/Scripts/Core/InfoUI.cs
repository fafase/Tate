using Rx;
using TMPro;
using Tools;
using UnityEngine;
using Zenject;

namespace Tatedrez.Core.UI
{
    public class InfoUI : MonoBehaviour
    {
        [Inject] private ICoreController m_core;
        [SerializeField] private TextMeshProUGUI m_text;

        private CompositeDisposable m_compositeDisposable = new CompositeDisposable();

        private void Start()
        {
            m_core.CurrentTurn.Subscribe(OnTurnChange).AddTo(m_compositeDisposable);
            Signal.Connect<EndGameSignal>(OnEndGame);
        }

        private void OnDestroy()
        {
            m_compositeDisposable?.Dispose();
            Signal.Disconnect<EndGameSignal>(OnEndGame);
        }

        private void OnTurnChange(Turn turn) 
        {
            m_text.text = turn.ToString();
        }
        private void OnEndGame() 
        {
            m_text.text = "Winner !";
        }
    }
}

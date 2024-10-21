using Tools;
using Tatedrez.UI;

namespace Tatedrez.Core
{
    public class CoreNavigation : Navigation
    {
        private bool m_endGame = false;
        protected override void Start()
        {
            base.Start();
            Signal.Connect<EndGameSignal>(OnEndGame);
            Signal.Connect<PawnMovementSignal>(OnPawnMovement);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Signal.Disconnect<EndGameSignal>(OnEndGame);
            Signal.Disconnect<PawnMovementSignal>(OnPawnMovement);
        }

        protected override IPopup OpenPopup() 
        {
            return m_popupManager.Show<SettingsPopup>(); 
        }

        private void OnEndGame()
        {
            m_loadButton.interactable = false;
            m_endGame = true;
        }

        private void OnPawnMovement(PawnMovementSignal data) 
        {
            if (m_endGame)
            {
                return;
            }
            m_loadButton.interactable = !data.StartMovement;
        }
    }
}

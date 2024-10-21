using Tools;
using Tatedrez.UI;

namespace Tatedrez.Core
{
    public class CoreNavigation : Navigation
    {
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
            Signal.Send(new PauseGameSignal(true));
            return m_popupManager.Show<SettingsPopup>(); 
        }

        private void OnEndGame()
        {
            m_loadButton.interactable = false;
        }

        private void OnPawnMovement(PawnMovementSignal data) 
        {
            m_loadButton.interactable = !data.StartMovement;
        }
    }
}

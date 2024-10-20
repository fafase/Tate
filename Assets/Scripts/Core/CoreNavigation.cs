using Tools;
using Tatedrez.UI;

namespace Tatedrez.Core
{
    public class CoreNavigation : Navigation
    {
        protected override IPopup OpenPopup() 
        {
            Signal.Send(new PauseGameSignal(true));
            return m_popupManager.Show<SettingsPopup>(); 
        }
    }
}

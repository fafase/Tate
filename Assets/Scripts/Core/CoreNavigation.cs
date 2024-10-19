using Tools;
using Tatedrez.UI;

namespace Tatedrez.Core
{
    public class CoreNavigation : Navigation
    {
        protected override IPopup OpenPopup() => m_popupManager.Show<QuitLevelPopup>();
    }
}

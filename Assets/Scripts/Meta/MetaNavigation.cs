using Tools;
using Tatedrez.UI;

namespace Tatedrez.Meta
{
    public class MetaNavigation : Navigation
    {
        protected override IPopup OpenPopup() => m_popupManager.Show<PlayLevelPopup>();
    }
}

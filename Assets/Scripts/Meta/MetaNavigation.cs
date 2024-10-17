using Tools;

public class MetaNavigation : Navigation
{
    protected override IPopup OpenPopup() => m_popupManager.Show<PlayLevelPopup>();
}


using Tools;

public class CoreNavigation : Navigation
{
    protected override IPopup OpenPopup() => m_popupManager.Show<QuitLevelPopup>();
}

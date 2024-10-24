using Tate.Loading;
using Tools;
using Tools.Audio;
using UnityEngine;
using Zenject;

public class ZenjectProjectContext : MonoInstaller
{
    [SerializeField] private GameObject m_popupManager;
    [SerializeField] private LoadingService m_loadingService;
    [SerializeField] private AudioManager m_audioManager;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<PopupManager>().FromComponentInNewPrefab(m_popupManager).AsSingle().NonLazy();
        Container.BindInterfacesTo<LoadingService>().FromComponentInNewPrefab(m_loadingService).AsSingle().NonLazy();
        Container.Bind<AudioManager>().FromComponentInNewPrefab(m_audioManager).AsSingle().NonLazy();

        Container.BindFactory<Popup, Popup, Popup.Factory>().FromFactory<PopupFactory>();
    }
}

public class PopupFactory : IFactory<Popup, Popup>
{
    readonly DiContainer m_container;

    public PopupFactory(DiContainer container)
    {
        m_container = container;
    }
    public Popup Create(Popup prefab)
    {
        return m_container.InstantiatePrefabForComponent<Popup>(prefab);
    }
}
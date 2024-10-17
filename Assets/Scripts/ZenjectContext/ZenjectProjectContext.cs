using Tools;
using UnityEngine;
using Zenject;

public class ZenjectProjectContext : MonoInstaller
{
    [SerializeField] private GameObject m_popupManager;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneLoading>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PopupManager>().FromComponentInNewPrefab(m_popupManager).AsSingle().NonLazy();

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
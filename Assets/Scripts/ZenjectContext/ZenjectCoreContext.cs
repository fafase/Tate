using UnityEngine;
using Zenject;

public class ZenjectCoreContext : MonoInstaller
{
    [SerializeField] private CoreController m_coreController; 
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ICoreController>().FromInstance(m_coreController);
    }
}

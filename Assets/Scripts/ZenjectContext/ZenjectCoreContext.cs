using UnityEngine;
using Zenject;

namespace Tatedrez.Core 
{
    public class ZenjectCoreContext : MonoInstaller
    {
        [SerializeField] private CoreController m_coreController;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ICoreController>().FromInstance(m_coreController);
        }
    }
}
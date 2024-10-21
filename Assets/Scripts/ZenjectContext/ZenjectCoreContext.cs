using UnityEngine;
using Zenject;

namespace Tatedrez.Core 
{
    public class ZenjectCoreContext : MonoInstaller
    {
        [SerializeField] private CoreController m_coreController;
        [SerializeField] private PawnMovementService m_movementService;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ICoreController>().FromInstance(m_coreController);
            Container.BindInterfacesAndSelfTo<IMovementService>().FromInstance(m_movementService);
            Container.BindInterfacesAndSelfTo<GridController>().FromNew().AsSingle().NonLazy();
        }
    }
}
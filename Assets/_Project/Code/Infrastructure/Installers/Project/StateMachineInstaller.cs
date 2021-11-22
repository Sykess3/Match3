using _Project.Code.Infrastructure.GameStates.Interfaces;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Project
{
    public class StateMachineInstaller : Installer<StateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<GameStateMachine>()
                .AsSingle();
            
            Container.Bind<IExitableGameState>()
                .To(x => x.AllNonAbstractClasses())
                .AsSingle();
        }
    }
}
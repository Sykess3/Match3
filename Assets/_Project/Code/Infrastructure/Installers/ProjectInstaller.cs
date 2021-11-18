using _Project.Code.Infrastructure.GameStates.Interfaces;
using _Project.Code.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            ProvidersInstaller.Install(Container);
            StateMachineInstaller.Install(Container);
            
            Container
                .BindInterfacesTo<GameBootstrapper>()
                .AsSingle();

            Container.BindInterfacesTo<ProjectInstaller>()
                .FromInstance(this)
                .AsSingle();
            
            Container.Bind<LoadingCurtain>()
                .FromComponentInNewPrefab(_settings.LoadingCurtain)
                .AsSingle();
            
            Container.Bind<Settings>()
                .FromInstance(_settings)
                .AsSingle();

            Container.Bind<LevelLoader>()
                .AsSingle();

            Container.Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();
        }
    }
}
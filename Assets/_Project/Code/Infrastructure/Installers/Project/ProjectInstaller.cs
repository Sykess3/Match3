using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure.Factories;
using _Project.Code.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Project
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

            Container
                .BindInterfacesTo<ProjectInstaller>()
                .FromInstance(this)
                .AsSingle();
            
            Container
                .Bind<LoadingCurtain>()
                .FromComponentInNewPrefab(_settings.LoadingCurtain)
                .AsSingle();
            
            Container
                .Bind<Settings>()
                .FromInstance(_settings)
                .AsSingle();

            Container
                .Bind<SceneLoader>()
                .AsSingle();

            Container
                .Bind<ISceneLoader>()
                .To<InternalSceneLoader>()
                .AsSingle();
            
                    
            BindConfigs();
            
        }

        private void BindConfigs()
        {
            Container
                .Bind<IEnumerable<ICellContentConfig>>()
                .FromInstance(_settings.CellContentConfigs)
                .AsCached();

            BindBombConfigs();
        }

        private void BindBombConfigs()
        {
            IEnumerable<IBombConfig> bombConfigs =
                _settings.CellContentConfigs.Where(x => x is IBombConfig).Cast<IBombConfig>();

            Container
                .Bind<IEnumerable<IBombConfig>>()
                .FromInstance(bombConfigs)
                .AsCached();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure.Factories;
using _Project.Code.Infrastructure.Loading;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Project
{
    public class ProjectInstaller : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private Settings _settings;

        public override void InstallBindings()
        {
            LoadInstaller.Install(Container);

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
                .Bind<LevelLoader>()
                .AsSingle();

            Container
                .Bind<IUnitySceneLoader>()
                .To<UnitySceneLoader>()
                .AsSingle();
        }
        
    }
}
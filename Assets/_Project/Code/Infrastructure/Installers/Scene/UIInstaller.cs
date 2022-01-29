using _Project.Code.Infrastructure.Factories.Meta.UI;
using _Project.Code.Meta.Configs;
using _Project.Code.Meta.Models.UI;
using _Project.Code.Meta.Models.UI.Interfaces;
using _Project.Code.Meta.Views.UI;
using _Project.Code.Meta.Views.UI.Markers;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private WindowsConfigs _configs;
        [SerializeField] private UIRoot _root;

        public override void InstallBindings()
        {
            Container
                .Bind<UIRoot>()
                .FromInstance(_root)
                .AsSingle();
            
            Container
                .Bind<IWindowsConfigs>()
                .To<WindowsConfigs>()
                .FromInstance(_configs)
                .AsSingle();

            Container
                .BindInterfacesTo<GameProgressWatcher>()
                .AsSingle();

            Container
                .Bind<IUIFactory>()
                .To<UIFactory>()
                .AsSingle();

            Container
                .Bind<WindowsService>()
                .AsSingle();

            Container
                .Bind<ReloadButton>()
                .AsSingle();
        }
        
    }
}
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class LevelConfigInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        public override void InstallBindings()
        {
            Container
                .Bind<ILevelConfig>()
                .FromInstance(_levelConfig)
                .AsSingle();
        }
    }
}
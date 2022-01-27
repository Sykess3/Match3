using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
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

            Container
                .Bind<IEnumerable<ICellContentConfig>>()
                .FromInstance(_levelConfig.CellContentConfigs)
                .AsCached();

            Container
                .Bind<IEnumerable<IContentDecoratorConfig>>()
                .FromInstance(_levelConfig.DecoratorsConfigs)
                .AsCached();

            Container
                .Bind<ILevelGoalConfig>()
                .FromInstance(_levelConfig.LevelGoal)
                .AsSingle();

            BindBombConfigs();
        }

        private void BindBombConfigs()
        {
            IEnumerable<IBombConfig> bombConfigs =
                _levelConfig.CellContentConfigs.Where(x => x is IBombConfig).Cast<IBombConfig>();

            Container
                .Bind<IEnumerable<IBombConfig>>()
                .FromInstance(bombConfigs)
                .AsCached();
        }
    }
}
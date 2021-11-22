using System.Collections.Generic;
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Installers.Factories;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField] private BoardView _boardView;
        [SerializeField] private BoardConfig _boardConfig;
        public override void InstallBindings()
        {
            Container
                .Bind<IBoardConfig>()
                .FromInstance(_boardConfig)
                .AsSingle();

            Container
                .Bind<BoardView>()
                .FromComponentInNewPrefab(_boardView)
                .AsSingle();
            
            Container
                .Bind<BoardViewPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<Board>()
                .AsSingle();
            Container
                .Bind<IEnumerable<Cell>>()
                .FromFactory<CellsFactory>()
                .AsSingle();
            
        }
    }
}
using System;
using _Project.Code.Core;
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.Particles;
using _Project.Code.Core.Models.BoardLogic.Swap;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Models.Random;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Factories;
using _Project.Code.Infrastructure.Installers.Factories;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private BoardView _boardView;
        public override void InstallBindings()
        {
            Container
                .Bind<BoardView>()
                .FromInstance(_boardView)
                .AsSingle();
            
            Container
                .Bind<BoardViewPresenter>()
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<ICellContentFactory>()
                .To<CellContentFactory>()
                .AsSingle();

            BindObjectPools();

#if UNITY_EDITOR
            Container
                .Bind<CellCollection>()
                .FromFactory<CellCollectionFactory>()
                .AsSingle();
            
            Container
                .Bind<ICellContentSpawner>()
                .To<CellContentSpawner>()
                .AsSingle();
            
            Container
                .Bind<IRandomCellContentGenerator>()
                .To<RandomCellContentGenerator>()
                .AsSingle();
#endif

            Container
                .Bind(typeof(Board), typeof(IInitializable), typeof(IDisposable))
                .To<Board>()
                .FromSubContainerResolve()
                .ByInstaller<BoardInstaller>()
                .AsSingle();
        }

        private void BindObjectPools()
        {
            Container
                .Bind<ICellContentPool>()
                .To<CellContentPool>()
                .AsSingle();

            Container
                .Bind<IParticlesPool>()
                .To<ParticlesPool>()
                .AsSingle();
        }
    }
}
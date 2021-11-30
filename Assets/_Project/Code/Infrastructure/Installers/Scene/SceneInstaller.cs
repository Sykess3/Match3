﻿using System;
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.Swap;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Models.Random;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Installers.Project;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class SceneInstaller : MonoInstaller
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
                .Bind(typeof(Board), typeof(IInitializable), typeof(IDisposable))
                .To<Board>()
                .FromSubContainerResolve()
                .ByInstaller<BoardInstaller>()
                .AsSingle();

        }

        class BoardInstaller : Installer<BoardInstaller>
        {
            public override void InstallBindings()
            {

                Container
                    .Bind(typeof(Board), typeof(IInitializable), typeof(IDisposable))
                    .To<Board>()
                    .AsSingle();

                Container
                    .Bind<IRandomCellContentGenerator>()
                    .To<RandomCellContentGenerator>()
                    .AsSingle();

                Container
                    .Bind<CellContentFallingFactory>()
                    .AsSingle();

                Container
                    .Bind<ICellContentMover>()
                    .To<CellContentMovement>()
                    .AsCached();

                Container
                    .Bind<ICellContentSwapper>()
                    .To<CellContentMovement>()
                    .AsCached();

                Container
                    .Bind<IContentMatcher>()
                    .To<ContentMatcher>()
                    .AsSingle();

                Container
                    .Bind<SwapCommandHandlerFactory>()
                    .AsSingle();

                Container
                    .Bind<CellCollectionFactory>()
                    .AsSingle();
            }
        }
    }
}
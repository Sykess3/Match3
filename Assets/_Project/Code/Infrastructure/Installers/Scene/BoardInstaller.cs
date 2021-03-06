using System;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents;
using _Project.Code.Core.Models.BoardLogic.Gravity;
using _Project.Code.Core.Models.BoardLogic.Swap;
using _Project.Code.Core.Models.Random;
using _Project.Code.Infrastructure.Installers.Factories;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class BoardInstaller : Installer<BoardInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind(typeof(Board), typeof(IInitializable), typeof(IDisposable))
                .To<Board>()
                .AsSingle();

            Container
                .Bind<MatchDataHandler>()
                .AsSingle();

            FallingBindings();

            MovementBindings();

            MatchFinderMiddlewareBindings();

            Container
                .Bind<SwapCommandHandler>()
                .AsSingle();
        }

        private void FallingBindings()
        {
            Container
                .Bind<BoardGravity>()
                .AsSingle();

            Container
                .Bind<ICellContentFalling>()
                .To<CellContentFalling>()
                .AsSingle();
        }

        private void MovementBindings()
        {
            Container
                .Bind<ICellContentMover>()
                .To<CellContentMovement>()
                .AsCached();

            Container
                .Bind<ICellContentSwapper>()
                .To<CellContentMovement>()
                .AsCached();
        }

        private void MatchFinderMiddlewareBindings()
        {

            Container
                .Bind<IContentMatchFinder>()
                .To<ContentMatchFinderMiddleware>()
                .AsSingle();

            Container
                .Bind<DefaultContentMatchFinder>()
                .AsSingle();

            Container
                .Bind<UppedContentMatchFinder>()
                .AsSingle();

            Container
                .Bind<BombMatchFinder>()
                .AsSingle();

            Container
                .Bind<DecoratorsFinder>()
                .AsSingle();
        }
    }

    public class MatchFinderMiddlewareInstaller : Installer<MatchFinderMiddlewareInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IContentMatchFinder>()
                .To<ContentMatchFinderMiddleware>()
                .AsSingle();

            Container
                .Bind<DefaultContentMatchFinder>()
                .AsSingle();

            Container
                .Bind<UppedContentMatchFinder>()
                .AsSingle();
        }
    }
}
using _Project.Code.Core.Models;
using _Project.Code.Infrastructure.Loading;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Project
{
    public class LoadInstaller : Installer<LoadInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();

            Container
                .Bind<IPersistentProgressLoader>()
                .To<PersistentProgressLoader>()
                .AsSingle();

            Container
                .Bind<ILevelLoader>()
                .FromSubContainerResolve()
                .ByMethod(LevelLoaderSubContainer)
                .AsTransient();
        }

        private void LevelLoaderSubContainer(DiContainer obj)
        {
            Container
                .Bind<ILevelLoader>()
                .To<LevelLoader>()
                .AsTransient();

            Container
                .Bind<IUnitySceneLoader>()
                .To<UnitySceneLoader>()
                .AsSingle();
        }
    }
}
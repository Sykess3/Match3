using _Project.Code.Infrastructure.Services;
using Zenject;

namespace _Project.Code.Infrastructure.Installers
{
    public class ProvidersInstaller : Installer<ProvidersInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();

            Container.Bind<IConfigProvider>()
                .To<ScriptableObjectProvider>()
                .AsSingle();
        }
    }
}
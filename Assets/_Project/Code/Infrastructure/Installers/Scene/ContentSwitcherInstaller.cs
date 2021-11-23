using _Project.Code.Core.Models;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class ContentSwitcherInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ContentSwapper>()
                .AsSingle();

            Container
                .Bind<IContentMatcher>()
                .To<ContentMatcher>()
                .AsSingle();
        }
    }
}
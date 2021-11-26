using _Project.Code.Core.Models;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Project
{
    public class CellContentMovementInstaller : MonoInstaller
    {
        public override void InstallBindings()
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
    }
}
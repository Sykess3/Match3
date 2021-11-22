using _Project.Code.Core.Input;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class SceneGameObjectsInstaller : MonoInstaller
    {
        [SerializeField] private Camera _cameraPrefab;
        public override void InstallBindings()
        {
            InputInstaller.Install(Container);
            Container
                .Bind<Camera>()
                .FromComponentInNewPrefab(_cameraPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
    
    public class InputInstaller : Installer<InputInstaller>
    {
        public override void InstallBindings()
        {
            var inputTransform = new GameObject("Input").transform;
            Container
                .Bind<IPlayerInput>()
                .To<PlayerInput>()
                .FromNewComponentOnNewGameObject()
                .UnderTransform(inputTransform)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<BoardInputHandler>()
                .FromNewComponentOnNewGameObject()
                .UnderTransform(inputTransform)
                .AsSingle()
                .NonLazy();
        }
    }
}
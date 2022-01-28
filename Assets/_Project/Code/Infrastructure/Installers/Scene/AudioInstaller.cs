using _Project.Code.Meta.Views.Hud;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class AudioInstaller : MonoInstaller<AudioInstaller>
    {
        [SerializeField] private AudioPlayer _player;
        public override void InstallBindings()
        {
            Container
                .Bind<AudioPlayer>()
                .FromInstance(_player)
                .AsSingle();
        }
    }
}
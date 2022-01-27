using _Project.Code.Infrastructure.Loading;
using Zenject;

namespace _Project.Code.Infrastructure
{
    public class GameBootstrapper : IInitializable
    {
        private readonly LoadingCurtain _loadingCurtain;
        private readonly LevelLoader _levelLoader;
        private readonly Settings _settings;
        private readonly IPersistentProgressLoader _persistentProgress;

        public GameBootstrapper(LoadingCurtain loadingCurtain, 
            LevelLoader levelLoader,
            Settings settings,
            IPersistentProgressLoader persistentProgress)
        {
            _loadingCurtain = loadingCurtain;
            _levelLoader = levelLoader;
            _settings = settings;
            _persistentProgress = persistentProgress;
        }
        public void Initialize()
        {
            _loadingCurtain.Show();
            _persistentProgress.Load(onLoaded: LoadFirstScene);
        }

        private void LoadFirstScene() => _levelLoader.Load(_settings.FirstSceneToLoad);
    }
}

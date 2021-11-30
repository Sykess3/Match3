using _Project.Code.Infrastructure.GameStates;
using Zenject;

namespace _Project.Code.Infrastructure
{
    public class GameBootstrapper : IInitializable
    {
        private readonly GameStateMachine _stateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly SceneLoader _sceneLoader;
        private readonly Settings _settings;

        public GameBootstrapper(GameStateMachine stateMachine, 
            LoadingCurtain loadingCurtain, 
            SceneLoader sceneLoader,
            Settings settings)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _settings = settings;
        }
        public void Initialize()
        {
            _loadingCurtain.Show();
            _stateMachine.Enter<Load_ProgressState>();
            _sceneLoader.Load(_settings.FirstSceneToLoad);
        }
    }
}

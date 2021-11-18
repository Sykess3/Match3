using _Project.Code.Infrastructure.GameStates;
using Zenject;

namespace _Project.Code.Infrastructure
{
    public class GameBootstrapper : IInitializable
    {
        private readonly GameStateMachine _stateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly LevelLoader _levelLoader;
        private readonly Settings _settings;

        public GameBootstrapper(GameStateMachine stateMachine, 
            LoadingCurtain loadingCurtain, 
            LevelLoader levelLoader,
            Settings settings)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
            _levelLoader = levelLoader;
            _settings = settings;
        }
        public void Initialize()
        {
            _loadingCurtain.Show();
            _stateMachine.Enter<Load_ProgressState>();
            _levelLoader.Load(_settings.FirstSceneToLoad);
        }
    }
}

using System;
using _Project.Code.Infrastructure.GameStates;

namespace _Project.Code.Infrastructure
{
    public class LevelLoader
    {
        private readonly GameStateMachine _stateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private Action _onLoaded;
        private bool _withCurtain;


        public LevelLoader(GameStateMachine stateMachine, LoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
        }

        public void Load(string name, Action onLoaded = null, bool withCurtain = true)
        {
            _withCurtain = withCurtain;
            _onLoaded = onLoaded;
            
            if (_withCurtain)
                _loadingCurtain.Show();
            
            _stateMachine.Enter<Load_LevelDataState, string>(name);

            var loadSceneState = _stateMachine.Enter<LoadSceneState, string>(name);
            loadSceneState.SceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(object sender, EventArgs eventArgs)
        {
            var loadSceneState = (LoadSceneState) sender;
            loadSceneState.SceneLoaded -= OnSceneLoaded;

            _onLoaded?.Invoke();
            if (_withCurtain) 
                _loadingCurtain.Hide();
        }
    }
}
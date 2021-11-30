using System;
using _Project.Code.Infrastructure.GameStates;

namespace _Project.Code.Infrastructure
{
    public class SceneLoader
    {
        private readonly GameStateMachine _stateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private Action _onLoaded;


        public SceneLoader(GameStateMachine stateMachine, LoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
        }

        public void Load(string name, Action onLoaded = null)
        {
            _onLoaded = onLoaded;

            _loadingCurtain.Show();

            _stateMachine.Enter<Load_LevelDataState, string>(name);

            var loadSceneState = _stateMachine.Enter<LoadLevelState, string>(name);
            loadSceneState.SceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(object sender, EventArgs eventArgs)
        {
            ((LoadLevelState) sender).SceneLoaded -= OnSceneLoaded;
            

            _loadingCurtain.Hide();
            _onLoaded?.Invoke();
        }
    }
}
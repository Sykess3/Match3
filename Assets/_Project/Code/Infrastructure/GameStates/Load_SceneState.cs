using System;
using _Project.Code.Infrastructure.GameStates.Interfaces;
using _Project.Code.Infrastructure.Services;

namespace _Project.Code.Infrastructure.GameStates
{
    public class LoadSceneState : IGameStateWithPayload<string>
    {
        private readonly ISceneLoader _sceneLoader;
        public event EventHandler SceneLoaded;

        public LoadSceneState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public void Enter(string payload)
        {
            _sceneLoader.Load(payload, OnLoaded);
        }

        private void OnLoaded() => SceneLoaded?.Invoke(this, EventArgs.Empty);

        public void Exit()
        {
            
        }
    }
}
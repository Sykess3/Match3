using System;
using _Project.Code.Core.Models;
using UnityEngine;

namespace _Project.Code.Infrastructure.Loading
{
    public class LevelLoader : ILevelLoader
    {
        private const string LevelsField = "LevelsField";
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IUnitySceneLoader _sceneLoader;
        
        public LevelLoader(LoadingCurtain loadingCurtain, IUnitySceneLoader sceneLoader)
        {
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
        }

        public void Load(string name, Action onLoaded = null)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(name, OnLoaded);

            void OnLoaded()
            {
                _loadingCurtain.Hide();
                onLoaded?.Invoke();
            }
        }

        public void LoadLevelsField() => Load(LevelsField);

        public void Reload(Action onLoaded = null)
        {
            //_loadingCurtain.Show();
            _sceneLoader.Reload(OnLoaded);
                
            void OnLoaded()
            {
                // _loadingCurtain.Hide();
                onLoaded?.Invoke();
            }
        }
    }
}
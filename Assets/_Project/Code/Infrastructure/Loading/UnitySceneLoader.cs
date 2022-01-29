using System;
using System.Collections;
using _Project.Code.Core.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Code.Infrastructure.Loading
{
    internal class UnitySceneLoader : IUnitySceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private string _currentScene;

        public UnitySceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        public void Reload(Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(ReloadScene(_currentScene, onLoaded));
        }

        private IEnumerator ReloadScene(string currentScene, Action onLoaded)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(currentScene);

            while (!waitNextScene.isDone)
                yield return null;
            onLoaded?.Invoke();
        }

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            _currentScene = nextScene;
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;
            onLoaded?.Invoke();
        }
    }
}
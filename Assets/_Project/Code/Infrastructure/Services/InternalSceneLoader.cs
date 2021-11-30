using System;
using System.Collections;
using _Project.Code.Core.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Code.Infrastructure.Services
{
    internal class InternalSceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public InternalSceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
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
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Code.Core.Models.Debug
{
    public class EventFunctions_DebugCreator : MonoBehaviour
    {
#if UNITY_EDITOR
        
        private void Awake()
        {
            SceneManager.sceneLoaded += CreateEventFunctions_Debug;
        }

        private void CreateEventFunctions_Debug(Scene arg0, LoadSceneMode arg1)
        {
            if (arg1 == LoadSceneMode.Single)
                new GameObject("EventFunctions_Debug").AddComponent<EventFunctions_Debug>();
        }
#endif
    }

    public class EventFunctions_Debug : MonoBehaviour
    {
        public static event Action OnStart;
        public static event Action OnUpdate;
        public void Start()
        {
            OnStart?.Invoke();
        }

        public void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
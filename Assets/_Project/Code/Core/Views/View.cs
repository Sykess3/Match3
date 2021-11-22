using System;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    public class View : MonoBehaviour
    {
        public event Action Destroyed;
        public event Action Created;


        private void Start()
        {
            Created?.Invoke();
            
            OnStart();
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
            
            OnDestroyed();
        }

        protected virtual void OnDestroyed() { }
        protected virtual void OnStart() { }
    }
}
using System;
using UnityEngine;

namespace _Project.Code.Core.Views.Helpers
{
    public class ParticlesDisabledEvent : MonoBehaviour
    {
        public event Action OnStopped;
        private void OnParticleSystemStopped() => OnStopped?.Invoke();
    }
}
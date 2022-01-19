using System;
using UnityEngine;

namespace _Project.Code.Core.Views.Particles
{
    public class ParticlesDisabledEvent : MonoBehaviour
    {
        public event Action OnStopped;
        private void OnParticleSystemStopped() => OnStopped?.Invoke();
    }
}
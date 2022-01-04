using System;
using _Project.Code.Core.Views.Helpers;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    [SelectionBase]
    public class CellContentView : View , IAnimatorEvent
    {
        [SerializeField] private ParticleSystem _matchVFX;

        [SerializeField] protected Animator Animator;
        private static readonly int FadeIn = Animator.StringToHash("FadeIn");

        public event Action Disabled;
        public event Action Enabled;
        
        public void Disable()
        {
            gameObject.SetActive(false);
            Disabled?.Invoke();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            Enabled?.Invoke();
        }


        public void Match() => Animator.SetTrigger(FadeIn);
        public void OnEventExecute() => Disable();
    }
}
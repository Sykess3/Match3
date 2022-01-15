using System;
using _Project.Code.Core.Views.Helpers;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    [SelectionBase]
    public class CellContentView : View, IAnimatorEvent
    {
        [SerializeField] protected Animator Animator;

        [SerializeField] private string[] _animationParametersNames;

        [SerializeField] private int[] _hashedAnimationStringsNames;

        private int _nextAnimationParameterIndex;

        private void OnValidate() => HashAnimationParameters();

        protected override void OnStart() =>
            Array.Clear(_animationParametersNames, 0, _animationParametersNames.Length - 1);

        public event Action Disabled;

        public void Enable()
        {
            Animator.SetBool(_hashedAnimationStringsNames[_nextAnimationParameterIndex], false);
            gameObject.SetActive(true);
            _nextAnimationParameterIndex = 0;
        }

        private void Disable() => Disabled?.Invoke();

        public void Match()
        {
            var isLastOrFirstAnimationParameter = _nextAnimationParameterIndex == _animationParametersNames.Length - 1;
            if (isLastOrFirstAnimationParameter)
                PlayFirstOrLastAnimation();
            else
                PlayNextAnimation();
        }

        private void PlayNextAnimation()
        {
            int previousAnimation = _hashedAnimationStringsNames[_nextAnimationParameterIndex];
            Animator.SetBool(previousAnimation, false);
            
            _nextAnimationParameterIndex++;
            
            int currentAnimation = _hashedAnimationStringsNames[_nextAnimationParameterIndex];
            Animator.SetBool(currentAnimation, true);
        }

        private void PlayFirstOrLastAnimation() =>
            Animator.SetBool(_hashedAnimationStringsNames[_nextAnimationParameterIndex], true);

        public void OnEventExecute() => Disable();

        private void HashAnimationParameters()
        {
            _hashedAnimationStringsNames = new int[_animationParametersNames.Length];

            for (int i = 0; i < _animationParametersNames.Length; i++)
                _hashedAnimationStringsNames[i] = Animator.StringToHash(_animationParametersNames[i]);
        }
    }
}
using System;
using UnityEngine;

namespace _Project.Code.Core.Views.Animation
{
    public class Decorated_ContentAnimation : MonoBehaviour , IAnimatorStateReader
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Decorator_CellContentView _decoratedView;
        
        [SerializeField] private string[] _animationParametersNames;
        [SerializeField] private int[] _hashedAnimationParametersNames;
        
        private int _nextAnimationParameterIndex;
        private void Start() => 
            Array.Clear(_animationParametersNames, 0, _animationParametersNames.Length - 1);
        private void OnValidate() => HashAnimationParameters();
        
        private void HashAnimationParameters()
        {
            _hashedAnimationParametersNames = new int[_animationParametersNames.Length];
        
            for (int i = 0; i < _animationParametersNames.Length; i++)
                _hashedAnimationParametersNames[i] = Animator.StringToHash(_animationParametersNames[i]);
        }
        
        private void PlayNextAnimation()
        {
            int previousAnimation = _hashedAnimationParametersNames[_nextAnimationParameterIndex];
            _animator.SetBool(previousAnimation, false);
            
            _nextAnimationParameterIndex++;
            
            int currentAnimation = _hashedAnimationParametersNames[_nextAnimationParameterIndex];
            _animator.SetBool(currentAnimation, true);
        }
        private void PlayFirstOrLastAnimation() =>
            _animator.SetBool(_hashedAnimationParametersNames[_nextAnimationParameterIndex], true);

        private void OnEnabled()
        {
            _animator.SetBool(_hashedAnimationParametersNames[_nextAnimationParameterIndex], false);
            gameObject.SetActive(true);
        }

        private void OnMatch()
        {
            var isLastOrFirstAnimationParameter = _nextAnimationParameterIndex == _animationParametersNames.Length - 1;
            if (isLastOrFirstAnimationParameter)
                PlayFirstOrLastAnimation();
            else
                PlayNextAnimation();
        }

        public void EnteredState(int hash)
        {
            
        }

        public void ExitedState(int hash)
        {
            
        }
    }
}
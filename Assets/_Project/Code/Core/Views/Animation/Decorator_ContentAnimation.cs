using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Code.Core.Views.Animation
{
    public class Decorator_ContentAnimation : MonoBehaviour , IAnimatorStateReader
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Decorator_CellContentView _decoratedView;
        
        [SerializeField] private StringPair[] _names;
        [SerializeField] private IntPair[] _hashed;
        
        private int _nextAnimationParameterIndex;

        private void Start() => _decoratedView.Matched += OnMatch;

        private void OnDestroy() => _decoratedView.Matched -= OnMatch;

        private void OnValidate() => HashAnimationParameters();

        private void OnMatch()
        {
            var isFirst = _nextAnimationParameterIndex == 0;
            var isLast = _nextAnimationParameterIndex == _names.Length - 1;
            if (isFirst)
                PlayFirst();
            else if(isLast)
                PlayLast();
            else
                PlayNext();
        }

        private void PlayLast()
        {
            int previousAnimation = _hashed[_nextAnimationParameterIndex - 1].ParameterName;
            _animator.SetBool(previousAnimation, false);

            int nextAnimation = _hashed[_nextAnimationParameterIndex].ParameterName;
            _animator.SetBool(nextAnimation, true);
            
        }

        private void PlayNext()
        {
            int previousAnimation = _hashed[_nextAnimationParameterIndex].ParameterName;
            _animator.SetBool(previousAnimation, false);
            
            _nextAnimationParameterIndex++;
            
            int currentAnimation = _hashed[_nextAnimationParameterIndex].ParameterName;
            _animator.SetBool(currentAnimation, true);
        }

        private void PlayFirst()
        {
            _animator.SetBool(_hashed[_nextAnimationParameterIndex].ParameterName, true);
            _nextAnimationParameterIndex++;
        }

        public void EnteredState(int hash, float playDuration)
        {
            if (hash == _hashed.Last().AnimationName)
                StartCoroutine(AfterAnimationEnd(playDuration, callback: _decoratedView.Destroy));
            else
                StartCoroutine(AfterAnimationEnd(playDuration, callback: _decoratedView.OnAnimationEnded));

        }

        private IEnumerator AfterAnimationEnd(float playDuration, Action callback)
        {
            yield return new WaitForSeconds(playDuration);
            
            callback?.Invoke();
        }

        public void ExitedState(int hash, float playDuration)
        { }

        private void HashAnimationParameters()
        {
            _hashed = new IntPair[_names.Length];

            for (int i = 0; i < _names.Length; i++)
            {
                _hashed[i] = new IntPair
                {
                    AnimationName = Animator.StringToHash(_names[i].AnimationName),
                    ParameterName = Animator.StringToHash(_names[i].ParameterName)
                };
            }
        }

        [Serializable]
        class StringPair
        {
            public string ParameterName;
            public string AnimationName;
        }

        [Serializable]
        class IntPair
        {
            public int ParameterName;
            public int AnimationName;
        }
    }
}
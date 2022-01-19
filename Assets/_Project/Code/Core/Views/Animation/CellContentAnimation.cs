using System;
using UnityEngine;

namespace _Project.Code.Core.Views.Animation
{
    public class CellContentAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CellContentView _view;
        [SerializeField] private string _animationParameterName;
        [SerializeField] private int _hashedAnimationParameterName;

        private void Start()
        {
            _view.Matched += PlayAnimation;
            _view.Enabled += SetIdleAnimation;
        }

        private void OnDestroy()
        {
            _view.Matched -= PlayAnimation;
            _view.Enabled -= SetIdleAnimation;
        }

        private void SetIdleAnimation() => _animator.SetBool(_hashedAnimationParameterName, false);

        private void OnValidate() => HashAnimationParameters();

        private void PlayAnimation() => _animator.SetBool(_hashedAnimationParameterName, true);
        
        private void HashAnimationParameters() => _hashedAnimationParameterName = Animator.StringToHash(_animationParameterName);

        private void OnFadeInEnded_Event() => _view.Disable();
    }
}
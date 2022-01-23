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

        private readonly int _clickParameter = Animator.StringToHash("Click");

        private void Start()
        {
            _view.Matched += PlayMatch;
            _view.Enabled += SetIdle;
            _view.Selected += OnSelect;
            _view.Deselected += OnDeselect;

            _animator.keepAnimatorControllerStateOnDisable = true;
        }

        private void OnDeselect() => _animator.SetBool(_clickParameter, false);

        private void OnSelect() => _animator.SetBool(_clickParameter, true);

        private void OnDestroy()
        {
            _view.Matched -= PlayMatch;
            _view.Enabled -= SetIdle;
            _view.Selected -= OnSelect;
            _view.Deselected -= OnDeselect;
        }

        private void SetIdle() => _animator.SetBool(_hashedAnimationParameterName, false);

        private void OnValidate() => HashAnimationParameters();

        private void PlayMatch() => _animator.SetBool(_hashedAnimationParameterName, true);

        private void HashAnimationParameters() =>
            _hashedAnimationParameterName = Animator.StringToHash(_animationParameterName);

        private void OnFadeInEnded_Event()
        {
            _view.Disable();
        }
    }
}
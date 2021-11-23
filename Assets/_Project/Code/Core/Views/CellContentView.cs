using System;
using _Project.Code.Core.Input;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Views
{
    public class CellContentView : View
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Range(0, 1)]
        [SerializeField] private float _movementDuration;
        
        private BoardInputHandler _inputHandler;
        private TweenerCore<Vector3, Vector3, VectorOptions> _currentTween;

        [Inject]
        private void Construct(BoardInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }
        

        public void ChangePosition(Vector2 position)
        {
            if (_currentTween.IsPlaying())
            {
                _currentTween.SetLoops(1, LoopType.Yoyo)
                    .OnComplete(SwitchOnInput);
            }
            
            SwitchOffInput();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> MoveTo(Vector2 position)
        {
            return transform.DOMove(position, _movementDuration);
        }

        private void SwitchOffInput()
        {
            
        }

        private void SwitchOnInput()
        {
            
        }
    }
}
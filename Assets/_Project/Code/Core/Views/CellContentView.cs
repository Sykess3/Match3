using _Project.Code.Core.Input;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Views
{
    public class CellContentView : View
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private BoardInputHandler _inputHandler;

        [Inject]
        private void Construct(BoardInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }
        public Sprite Sprite
        {
            set => _spriteRenderer.sprite = value;
        }
    }
}
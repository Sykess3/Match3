using UnityEngine;

namespace _Project.Code.Core.Views
{
    public class CellContentView : View
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Sprite Sprite
        {
            set => _spriteRenderer.sprite = value;
        }
    }
}
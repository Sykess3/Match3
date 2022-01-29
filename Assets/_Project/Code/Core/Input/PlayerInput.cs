using System;
using _Project.Code.Core.Models;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Input
{
    public class PlayerInput : MonoBehaviour, IPlayerInput
    {
        public event Action<Vector2> ClickedOnPosition;

        private Camera _camera;
        private bool _disabled;

        [Inject]
        public void Construct(Camera camera)
        {
            _camera = camera;
        }

        private void Update()
        {
            if (_disabled)
                return;

            if (Clicked(out var position))
            {
                ClickedOnPosition?.Invoke(position);
            }
        }

        public void Disable() => _disabled = true;

        public void Enable() => _disabled = false;

        private bool Clicked(out Vector2 position)
        {
            if (!UnityEngine.Input.GetMouseButtonDown(0))
            {
                position = Vector2.zero;
                return false;
            }

            var mousePosition = UnityEngine.Input.mousePosition;
            position = _camera.ScreenToWorldPoint(mousePosition);
            return true;
        }
    }
}
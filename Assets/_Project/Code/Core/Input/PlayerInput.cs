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

        [Inject]
        public void Construct(Camera camera)
        {
            _camera = camera;
        }

        private void Update()
        {
            if (ClickedOnClickableGameObject(out var position))
            {
                ClickedOnPosition?.Invoke(position);
            }
        }

        private bool ClickedOnClickableGameObject(out Vector2 position)
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
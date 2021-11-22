using System;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    public class UpdatableView : View
    {
        public event Action<float> OnUpdate;
        private void Update() => OnUpdate?.Invoke(Time.deltaTime);
    }
}
using System;
using _Project.Code.Core.Views.Animation;
using _Project.Code.Core.Views.Framework;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    [SelectionBase]
    public class DefaultCellContentView : View, ICellContentView
    {
        public event Action Selected;
        public event Action AnimationEnded;
        public event Action Enabled;
        public event Action Matched;

        public event Action Deselected;
        
        public void Enable()
        {
            gameObject.SetActive(true);
            Enabled?.Invoke();
        }

        public void Select() => Selected?.Invoke();
        public void Deselect() => Deselected?.Invoke();

        public void Match() => Matched?.Invoke();
        
        public void Disable()
        {
            gameObject.SetActive(false);
            AnimationEnded?.Invoke();
        }
    }
}
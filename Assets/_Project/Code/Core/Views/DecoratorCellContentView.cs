using System;
using _Project.Code.Core.Views.Framework;

namespace _Project.Code.Core.Views
{
    public class DecoratorCellContentView : View, ICellContentView
    {
        public event Action AnimationEnded;
        public event Action Matched;


        public void Match() => Matched?.Invoke();

        public void OnAnimationEnded() => AnimationEnded?.Invoke();

        public void Destroy() => Destroy(gameObject);
    }
}
using System;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Presenters.Framework;
using _Project.Code.Core.Views;

namespace _Project.Code.Core.Presenters
{
    public class DecoratorCellContentViewPresenter : Presenter<DecoratorCellContent, DecoratorCellContentView>
    {
        private int _currentIndex;

        public DecoratorCellContentViewPresenter(DecoratorCellContent[] decorators, DecoratorCellContentView view) :
            base(
                decorators, view)
        {
            _currentIndex = decorators.Length - 1;
        }

        protected override void OnStart() => SyncPosition();

        protected override void Subscribe() => SubscribeInternal(Models[_currentIndex]);

        protected override void CleanUp() => UnSubscribeInternal(Models[_currentIndex]);

        private void SubscribeInternal(DecoratorCellContent model)
        {
            model.Matched += View.Match;
            model.PositionChanged += SyncPosition;
            View.AnimationEnded += OnAnimationEnded;
            View.Destroyed += model.Disable;
        }

        private void UnSubscribeInternal(DecoratorCellContent model)
        {
            model.Matched -= View.Match;
            model.PositionChanged -= SyncPosition;
            View.AnimationEnded -= OnAnimationEnded;
            View.Destroyed -= model.Disable;
        }

        private void OnAnimationEnded()
        {
            Models[_currentIndex].Disable();
            UnSubscribeInternal(Models[_currentIndex]);

            _currentIndex--;

            SubscribeInternal(Models[_currentIndex]);
        }


        private void SyncPosition() => View.transform.position = Models[_currentIndex].Position;
    }
}
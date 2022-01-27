using System;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.Framework;

namespace _Project.Code.Meta.Models.Hud
{
    public abstract class SingleGoal<T> : IModel where T : Enum 
    {
        private (T, int) _currentProgress;
        protected readonly Board Board;

        public int CurrentProgress => _currentProgress.Item2;
        public event Action<int> Collected;

        public SingleGoal(Board board, (T, int) goal)
        {
            Board = board;
            _currentProgress = goal;
        }

        public abstract void Subscribe();
        public abstract void CleanUp();

        protected void TryCollect(T type)
        {
            if (_currentProgress.Item2 == 0)
                return;
            
            if (Equals(_currentProgress.Item1, type))
            {
                _currentProgress.Item2--;
                Collected?.Invoke(_currentProgress.Item2);
            }
        }
    }
}
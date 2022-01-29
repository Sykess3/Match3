using System;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.Framework;
using Zenject;

namespace _Project.Code.Meta.Models.Hud
{
    public class GoalCalculator : IInitializable, IDisposable, IModel
    {
        private readonly Board _board;
        private readonly IGoalsFactory _factory;
        
        private DefaultSingleGoal[] _defaultSingleGoals;
        private DecoratorSingleGoal[] _decoratorSingleGoals;
        
        public bool IsReached { get; private set; }

        public event Action Reached;
        
        public GoalCalculator(Board board, IGoalsFactory factory)
        {
            _board = board;
            _factory = factory;
        }

        void IInitializable.Initialize()
        {
            _defaultSingleGoals = _factory.CreateDefault();
            _decoratorSingleGoals = _factory.CreateDecorator();

            _board.FallingEnded += OnFallingEnded;
        }

        void IDisposable.Dispose()
        {
            _board.FallingEnded -= OnFallingEnded;
        }

        private void OnFallingEnded()
        {
            
            int reachedSingleGoals = GetReachedGoals(_decoratorSingleGoals);
            reachedSingleGoals += GetReachedGoals(_defaultSingleGoals);

            int goalsCount = _decoratorSingleGoals.Length + _defaultSingleGoals.Length;
            if (reachedSingleGoals == goalsCount)
            {
                IsReached = true;
                Reached?.Invoke();
            }

        }

        private static int GetReachedGoals<T>(SingleGoal<T>[] decoratorSingleGoals) where T : Enum
        {
            int reachedSingleGoals = 0;
            foreach (var singleGoal in decoratorSingleGoals)
            {
                if (singleGoal.CurrentProgress == 0)
                {
                    reachedSingleGoals++;
                }
            }

            return reachedSingleGoals;
        }
    }
}
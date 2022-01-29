using System;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Meta.Models.Hud;
using Zenject;

namespace _Project.Code.Meta.Models.UI
{
    public class GameProgressWatcher : IInitializable, IDisposable
    {
        private readonly GoalCalculator _goalCalculator;
        private readonly SwapCounter _swapCounter;
        private readonly WindowsService _windowsService;
        private readonly IPlayerInput _input;
        private readonly Board _board;

        private bool _isReachedGoal;
        private bool _isRequestedToOpenEndGameWindow;

        public GameProgressWatcher(GoalCalculator goalCalculator,
            SwapCounter swapCounter,
            WindowsService windowsService,
            IPlayerInput input,
            Board board)
        {
            _goalCalculator = goalCalculator;
            _swapCounter = swapCounter;
            _windowsService = windowsService;
            _input = input;
            _board = board;
        }


        public void Initialize()
        {
            _goalCalculator.Reached += OnGoalReached;
            _swapCounter.SwapsEnded += OnSwapsEnded;
        }

        public void Dispose()
        {
            _goalCalculator.Reached -= OnGoalReached;
            _swapCounter.SwapsEnded -= OnSwapsEnded;
        }

        private void OnSwapsEnded()
        {
            if (!_isRequestedToOpenEndGameWindow) 
                _board.FallingEnded += OpenEndGameWindow;

            _isRequestedToOpenEndGameWindow = true;
        }

        private void OnGoalReached()
        {
            _isReachedGoal = true;
            if (!_isRequestedToOpenEndGameWindow) 
                OpenEndGameWindow();
        }

        private void OpenEndGameWindow()
        {
            _board.FallingEnded -= OpenEndGameWindow;

            if (_isReachedGoal)
                OpenWindow(WindowId.Win);
            else
                OpenWindow(WindowId.Lose);
        }

        private void OpenWindow(WindowId id)
        {
            _input.Disable();
            _windowsService.Open(id);
        }
    }
}
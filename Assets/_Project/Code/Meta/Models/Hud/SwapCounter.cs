using System;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.Framework;
using UnityEngine;
using Zenject;

namespace _Project.Code.Meta.Models.Hud
{
    public class SwapCounter : IModel
    {
        private readonly BoardInputHandler _boardInputHandler;
        private readonly GoalCalculator _goalCalculator;
        public int SwapsLeft { get; private set; }

        public event Action SwapsEnded;
        public event Action<int> Swapped;

        public SwapCounter(BoardInputHandler boardInputHandler, int maxSwapCount, GoalCalculator goalCalculator)
        {
            _boardInputHandler = boardInputHandler;
            _goalCalculator = goalCalculator;
            SwapsLeft = maxSwapCount;
        }

        public void Subscribe() => _boardInputHandler.Swapped += ChekIsItLastSwap;

        public void CleanUp() => _boardInputHandler.Swapped -= ChekIsItLastSwap;

        private void ChekIsItLastSwap()
        {
            SwapsLeft--;
            Swapped?.Invoke(SwapsLeft);

            if (SwapsLeft == 0 && !_goalCalculator.IsReached) 
                SwapsEnded?.Invoke();
        }
    }
}
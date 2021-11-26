using System;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.Cells;
using DG.Tweening;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public class SwapCommand
    {
        private readonly ICellContentSwapper _swapper;
        private Action<SwapCommand> _onCommandExecutedCallBack;
        private Action<SwapCommand> _onCommandRevertedCallBack;
        public Cell FirstCell { get; }
        public Cell SecondCell { get; }
        public float Speed { get; set; } = 2;

        public SwapCommand(
            Cell firstCell, 
            Cell secondCell,
            ICellContentSwapper swapper)
        {
            _swapper = swapper;
            FirstCell = firstCell;
            SecondCell = secondCell;
        }

        public void Execute(Action<SwapCommand> onCommandExecuted)
        {
            _onCommandExecutedCallBack = onCommandExecuted;
            _swapper.SwapContent(
                FirstCell,
                SecondCell,
                Speed,
                InvokeOnCommandExecuted);
        }

        public void Revert(Action<SwapCommand> onCommandRevertedCallBack = null)
        {
            _onCommandRevertedCallBack = onCommandRevertedCallBack;
            _swapper.SwapContent(
                FirstCell,
                SecondCell,
                Speed,
                InvokeOnCommandReverted);
        }

        private void InvokeOnCommandExecuted()
        {
            _onCommandExecutedCallBack?.Invoke(this);
        }

        private void InvokeOnCommandReverted()
        {
            _onCommandRevertedCallBack?.Invoke(this);
        }
    }
}
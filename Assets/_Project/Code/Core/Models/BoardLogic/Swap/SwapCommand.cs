using System;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic.Swap
{
    public class SwapCommand
    {
        private Action<SwapCommand> _onCommandExecutedCallBack;
        private Action<SwapCommand> _onCommandRevertedCallBack;
        public Cell FirstCell { get; }
        public Cell SecondCell { get; }
        public ICellContentSwapper Swapper { get; set; }

        public SwapCommand(
            Cell firstCell, 
            Cell secondCell)
        {
            FirstCell = firstCell;
            SecondCell = secondCell;
        }

        public void Execute(Action<SwapCommand> onCommandExecuted)
        {
            _onCommandExecutedCallBack = onCommandExecuted;
            Swapper.SwapContent(
                FirstCell,
                SecondCell,
                InvokeOnCommandExecuted);
        }

        public void Revert(Action<SwapCommand> onCommandRevertedCallBack = null)
        {
            _onCommandRevertedCallBack = onCommandRevertedCallBack;
            Swapper.SwapContent(
                FirstCell,
                SecondCell,
                InvokeOnCommandReverted);
        }

        private void InvokeOnCommandExecuted() => _onCommandExecutedCallBack?.Invoke(this);

        private void InvokeOnCommandReverted() => _onCommandRevertedCallBack?.Invoke(this);
    }
}
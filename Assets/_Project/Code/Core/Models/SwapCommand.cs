using System;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.Cells;
using DG.Tweening;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public class SwapCommand
    {
        private bool _oneContentReachedTarget;
        
        private Action<SwapCommand> _onCommandExecutedCallBack;
        public Cell FirstCell { get; }
        public Cell SecondCell { get; }
        public float Duration { get; set; } = 1;

        public SwapCommand(Cell firstCell, Cell secondCell)
        {
            FirstCell = firstCell;
            SecondCell = secondCell;
        }

        public void Execute(Action<SwapCommand> onCommandExecuted)
        {
            _onCommandExecutedCallBack = onCommandExecuted;

            var swapInstances = SwapPositions(
                firstFillerTargetPosition: SecondCell.Position,
                secondFillerTargetPosition: FirstCell.Position);

            swapInstances
                .Item1
                .OnComplete(TryInformExecutedSubscribers);

            swapInstances
                .Item2
                .OnComplete(TryInformExecutedSubscribers);
        }

        public void Revert(Action<SwapCommand> onCommandRevertedCallBack)
        {
            var swapInstances = SwapPositions(
                firstFillerTargetPosition: SecondCell.Position,
                secondFillerTargetPosition: FirstCell.Position);

            swapInstances.Item1.OnComplete(delegate
            {
                SwapContent();
                onCommandRevertedCallBack?.Invoke(this);
            });
        }

        private (Tweener, Tweener) SwapPositions(Vector2 firstFillerTargetPosition, Vector2 secondFillerTargetPosition)
        {
            (Tweener, Tweener) result = (
                MoveFillerTo(
                    currentFiller: FirstCell.Filler,
                    targetPoint: firstFillerTargetPosition),
                MoveFillerTo(
                    currentFiller: SecondCell.Filler, 
                    targetPoint: secondFillerTargetPosition)
            );

            return result;
        }

        private Tweener MoveFillerTo(Cell.Content currentFiller, Vector2 targetPoint)
        {
            return DOVirtual.Vector3(
                @from: currentFiller.Position,
                to: targetPoint,
                duration: Duration,
                (x) => currentFiller.Position = x);
        }

        private void SwapContent()
        {
            var temp = FirstCell.Filler;
            FirstCell.Filler = SecondCell.Filler;
            SecondCell.Filler = temp;
        }

        private void TryInformExecutedSubscribers()
        {
            if (!_oneContentReachedTarget)
            {
                _oneContentReachedTarget = true;
                return;
            }
            
            SwapContent();
            _onCommandExecutedCallBack?.Invoke(this);
        }
    }
}
using System;
using System.Threading.Tasks;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.Swap;
using DG.Tweening;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    public class CellContentMovement : ICellContentSwapper, ICellContentMover
    {
        public async void MoveCellContent(Cell from, Cell to, float speed, Action callback = null)
        {
            var cachedContent = from.Content;
            await MoveContentTo(
                from.Content,
                to,
                speed,
                movementType: Ease.Linear);

            if (@from.Content == cachedContent)
                @from.SetContentToEmpty();
            callback?.Invoke();
        }

        public async void MoveCellContent(CellContent contentToMove, Cell to, float speed, Action callback = null)
        {
            await MoveContentTo(
                contentToMove: contentToMove,
                targetCell: to,
                speed: speed,
                movementType: Ease.Linear);

            to.Content = contentToMove;
            callback?.Invoke();
        }

        public async void SwapContent(Cell firstCell, Cell secondCell, float speed, Action callback = null)
        {
            var task1 = MoveContentTo(
                contentToMove: firstCell.Content,
                targetCell: secondCell,
                speed: speed,
                movementType: Ease.InOutSine,
                changeIsFalling: false);
            var task2 = MoveContentTo(
                contentToMove: secondCell.Content,
                targetCell: firstCell,
                speed: speed,
                movementType: Ease.InOutSine,
                changeIsFalling: false);

            await Task.WhenAll(task1, task2);

            callback?.Invoke();
        }


        private async Task MoveContentTo(
            CellContent contentToMove,
            Cell targetCell,
            float speed,
            Ease movementType,
            bool changeIsFalling = true)
        {
            if (changeIsFalling)
                contentToMove.IsFalling = true;

            await DOVirtual.Vector3(
                    @from: contentToMove.Position,
                    to: targetCell.Position,
                    duration: speed,
                    (x) => contentToMove.Position = x)
                .SetSpeedBased(true)
                .SetEase(movementType)
                .AsyncWaitForCompletion();

            targetCell.Content = contentToMove;
            if (changeIsFalling)
                contentToMove.IsFalling = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.Swap;
using DG.Tweening;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    public class CellContentMovement : ICellContentSwapper, ICellContentMover
    {
        public async void MoveCellContent(Cell from, Cell to, Action callback = null)
        {
            var cachedContent = from.Content;
            //TODO: NULLREF ABOVE
            await FallContent(
                contentToMove: from.Content,
                targetCell: to,
                speed: Constant.FallingSpeed,
                movementType: Ease.Linear);

            if (@from.Content == cachedContent)
                @from.SetContentToEmpty();
            callback?.Invoke();
        }

        public async void MoveCellContent(CellContent contentToMove, Cell to, Action callback = null)
        {
            await DoTweenMovement(
                contentToMove: contentToMove,
                targetPosition: to.Position,
                speed: Constant.FallingSpeed,
                movementType: Ease.Linear);

            to.Content = contentToMove;
            callback?.Invoke();
        }

        public async void MoveCellContent(Cell from, Cell to, ContentRoute route, Action callback = null)
        {
            var contentToMove = from.Content;
            
            await MoveContentByRoute(contentToMove, to, route);

            if (contentToMove == from.Content)
                @from.SetContentToEmpty();

            callback?.Invoke();
        }

        public async void MoveCellContent(CellContent contentToMove, Cell to, ContentRoute route,
            Action callback = null)
        {
            await MoveContentByRoute(contentToMove, to, route);
            
            callback?.Invoke();
        }

        public async void SwapContent(Cell firstCell, Cell secondCell, Action callback = null)
        {
            var task1 = DoTweenMovement(
                contentToMove: firstCell.Content,
                targetPosition: secondCell.Position,
                speed: Constant.SwapSpeed,
                movementType: Ease.InOutSine);
            var task2 = DoTweenMovement(
                contentToMove: secondCell.Content,
                targetPosition: firstCell.Position,
                speed: Constant.SwapSpeed,
                movementType: Ease.InOutSine);
            
            SpawnCellsContentLinks();

            await Task.WhenAll(task1, task2);

            callback?.Invoke();

            void SpawnCellsContentLinks()
            {
                var temp = firstCell.Content;
                firstCell.Content = secondCell.Content;
                secondCell.Content = temp;
            }
        }

        private static async Task MoveContentByRoute(CellContent contentToMove, Cell targetCell, ContentRoute route)
        {
            targetCell.Content = contentToMove;
            while (route.Count > 0)
            {
                Vector2 nextPoint = route.PopPoint();
                await DoTweenMovement(
                    contentToMove: contentToMove,
                    targetPosition: nextPoint,
                    speed: Constant.FallingSpeed,
                    movementType: Ease.Linear);
            }
        }


        private async Task FallContent(
            CellContent contentToMove,
            Cell targetCell,
            float speed,
            Ease movementType)
        {
            contentToMove.IsFalling = true;
            
            targetCell.Content = contentToMove;
            await DoTweenMovement(contentToMove, targetCell.Position, speed, movementType);
            
            contentToMove.IsFalling = false;
        }

        private static async Task DoTweenMovement(CellContent contentToMove, Vector2 targetPosition, float speed,
            Ease movementType)
        {
            await DOVirtual.Vector3(
                    @from: contentToMove.Position,
                    to: targetPosition,
                    duration: speed,
                    onVirtualUpdate: ChangePosition)
                .SetSpeedBased(true)
                .SetEase(movementType)
                .AsyncWaitForCompletion();

            void ChangePosition(Vector3 x)
            {
                contentToMove.Position = x;
            }
        }
    }
}
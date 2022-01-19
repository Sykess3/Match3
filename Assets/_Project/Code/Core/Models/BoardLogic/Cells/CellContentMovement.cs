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
                contentBaseToMove: from.Content,
                targetCell: to,
                speed: Constant.FallingSpeed,
                movementType: Ease.Linear);

            if (@from.Content == cachedContent)
                @from.SetContentToEmpty();
            callback?.Invoke();
        }

        public async void MoveCellContent(CellContentBase contentBaseToMove, Cell to, Action callback = null)
        {
            await DoTweenMovement(
                contentBaseToMove: contentBaseToMove,
                targetPosition: to.Position,
                speed: Constant.FallingSpeed,
                movementType: Ease.Linear);

            to.Content = contentBaseToMove;
            callback?.Invoke();
        }

        public async void MoveCellContent(Cell from, Cell to, ContentRoute route, Action callback = null)
        {
            var contentToMove = from.Content;
            
            contentToMove.IsFalling = true;
            await MoveContentByRoute(contentToMove, to, route);
            contentToMove.IsFalling = false;
            
            if (contentToMove == from.Content)
                @from.SetContentToEmpty();

            callback?.Invoke();
        }

        public async void MoveCellContent(CellContentBase contentBaseToMove, Cell to, ContentRoute route,
            Action callback = null)
        {
            await MoveContentByRoute(contentBaseToMove, to, route);
            
            callback?.Invoke();
        }

        public async void SwapContent(Cell firstCell, Cell secondCell, Action callback = null)
        {
            var task1 = DoTweenMovement(
                contentBaseToMove: firstCell.Content,
                targetPosition: secondCell.Position,
                speed: Constant.SwapSpeed,
                movementType: Ease.InOutSine);
            var task2 = DoTweenMovement(
                contentBaseToMove: secondCell.Content,
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

        private static async Task MoveContentByRoute(CellContentBase contentBaseToMove, Cell targetCell, ContentRoute route)
        {
            targetCell.Content = contentBaseToMove;
            while (route.Count > 0)
            {
                Vector2 nextPoint = route.PopPoint();
                await DoTweenMovement(
                    contentBaseToMove: contentBaseToMove,
                    targetPosition: nextPoint,
                    speed: Constant.FallingSpeed,
                    movementType: Ease.Linear);
            }
        }


        private async Task FallContent(
            CellContentBase contentBaseToMove,
            Cell targetCell,
            float speed,
            Ease movementType)
        {
            contentBaseToMove.IsFalling = true;
            
            targetCell.Content = contentBaseToMove;
            await DoTweenMovement(contentBaseToMove, targetCell.Position, speed, movementType);
            
            contentBaseToMove.IsFalling = false;
        }

        private static async Task DoTweenMovement(CellContentBase contentBaseToMove, Vector2 targetPosition, float speed,
            Ease movementType)
        {
            await DOVirtual.Vector3(
                    @from: contentBaseToMove.Position,
                    to: targetPosition,
                    duration: speed,
                    onVirtualUpdate: ChangePosition)
                .SetSpeedBased(true)
                .SetEase(movementType)
                .AsyncWaitForCompletion();

            void ChangePosition(Vector3 x)
            {
                contentBaseToMove.Position = x;
            }
        }
    }
}
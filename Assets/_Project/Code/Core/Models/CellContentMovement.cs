using System;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.Cells;
using DG.Tweening;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public class CellContentMovement : ICellContentSwapper, ICellContentMover
    {
        private readonly IPlayerInput _playerInput;
        private readonly Board _board;

        public CellContentMovement(IPlayerInput playerInput, Board board)
        {
            _playerInput = playerInput;
            _board = board;
        }

        public void MoveCellContent(Cell from, Cell to, float speed, Action callback = null)
        {
            var cachedContent = from.Content;
            MoveContentTo(
                from.Content,
                to,
                speed,
                movementType: Ease.Linear,
                onComplete: delegate
                {
                    if (@from.Content == cachedContent)
                        @from.SetContentToEmpty();
                    callback?.Invoke();
                });
        }

        public void MoveCellContent(CellContent contentToMove, Cell to, float speed, Action callback = null)
        {
            MoveContentTo(
                contentToMove: contentToMove,
                targetCell: to,
                speed: speed,
                movementType: Ease.Linear,
                onComplete: delegate
                {
                    to.Content = contentToMove;
                    callback?.Invoke();
                });
        }

        public void SwapContent(Cell firstCell, Cell secondCell, float speed, Action callback = null)
        {
            bool oneContentReachedTarget = false;
            MoveContentTo(
                contentToMove: firstCell.Content,
                targetCell: secondCell,
                speed: speed,
                movementType: Ease.InOutSine,
                changeIsFalling: false,
                onComplete: delegate
                {
 
                    if (oneContentReachedTarget) 
                        callback?.Invoke();
                    else
                        oneContentReachedTarget = true;
                });
            MoveContentTo(
                contentToMove: secondCell.Content,
                targetCell: firstCell,
                speed: speed,
                movementType: Ease.InOutSine,
                changeIsFalling: false,
                onComplete: delegate
                {

                    if (oneContentReachedTarget) 
                        callback?.Invoke();
                    else
                        oneContentReachedTarget = true;
                });
        }
        

        private void MoveContentTo(
            CellContent contentToMove,
            Cell targetCell,
            float speed,
            Ease movementType,
            Action onComplete = null,
            bool changeIsFalling = true)
        {
            _playerInput.Disable();
            
            if (changeIsFalling)
                contentToMove.IsFalling = true;
            
            DOVirtual.Vector3(
                    @from: contentToMove.Position,
                    to: targetCell.Position,
                    duration: speed,
                    (x) => contentToMove.Position = x)
                .OnComplete(delegate
                {
                    targetCell.Content = contentToMove;
                    if (changeIsFalling)
                        contentToMove.IsFalling = false;

                    EnableInputIfNoOneIsMoving();
                    onComplete?.Invoke();
                })
                .SetSpeedBased(true)
                .SetEase(movementType);
            
        }

        private void EnableInputIfNoOneIsMoving()
        {
            if (!_board.IsAnyContentMoving())
                _playerInput.Enable();
        }
        
    }
}
using System;
using System.Collections;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Random;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class CellContentFalling : ICellContentFalling
    {
        private const int YMinSpawnPosition = 5;
        private const float FallingSpeed = 6f;

        private readonly CellCollection _cellCollection;
        private readonly IRandomCellContentGenerator _contentGenerator;
        private readonly ICellContentMover _mover;
        private readonly int[] _ySpawnPositions = new int[Constant.Board.Size.x];
        private bool _isResetSpawnPositions;

        public CellContentFalling(CellCollection cellCollection, IRandomCellContentGenerator contentGenerator,
            ICellContentMover mover)
        {
            _cellCollection = cellCollection;
            _contentGenerator = contentGenerator;
            _mover = mover;

            ResetSpawnPositionsArray();
        }

        public void FillContentOnEmptyCell(Cell emptyCell, Action<Cell> onLandedCallback)
        {
            _isResetSpawnPositions = false;
            if (!TryMoveExistingContentToEmptyCells(emptyCell, onLandedCallback))
                GenerateNewContentWithMovementTo(emptyCell, onLandedCallback);
        }

        private void GenerateNewContentWithMovementTo(Cell emptyCell, Action<Cell> onLandedCallback)
        {
            var randomContent = _contentGenerator.Generate(
                new Vector2(emptyCell.Position.x, _ySpawnPositions[XIndexOnMatrixOfEmptyCell()]));

            _mover.MoveCellContent(
                contentToMove: randomContent,
                to: emptyCell,
                speed: FallingSpeed,
                callback: () => OnLandedGeneratedContent(emptyCell, onLandedCallback));

            _ySpawnPositions[XIndexOnMatrixOfEmptyCell()]++;

            int XIndexOnMatrixOfEmptyCell()
            {
                return Mathf.RoundToInt(emptyCell.Position.x + Constant.Board.OffsetFromCenter.x);
            }
        }

        private void OnLandedGeneratedContent(Cell emptyCell, Action<Cell> onLandedCallback)
        {
            if (!_isResetSpawnPositions)
            {
                _isResetSpawnPositions = true;
                ResetSpawnPositionsArray();
            }

            onLandedCallback?.Invoke(emptyCell);
        }

        private bool TryMoveExistingContentToEmptyCells(Cell emptyCell, Action<Cell> onLanded)
        {
            if (TryGetFilledCellAbove(emptyCell.Position, out var filledCell))
            {
                _mover.MoveCellContent(
                    @from: filledCell,
                    to: emptyCell,
                    speed: FallingSpeed,
                    callback: CallCallback);
                return true;
            }
            
            

            return false;

            void CallCallback()
            {
                onLanded(emptyCell);
            }
        }

        private bool TryGetFilledCellAbove(Vector2 position, out Cell filledCell)
        {
            filledCell = null;
            var currentPosition = position;
            while (_cellCollection.TryGetCellAbove(currentPosition, out var cellAbove))
            {
                if (cellAbove.Content.IsFalling)
                {
                    currentPosition = cellAbove.Position;
                    continue;
                }

                var contentIsImmovable = !cellAbove.Content.Switchable && cellAbove.Content.Type != ContentType.Empty;
                if (contentIsImmovable)
                    return false;

                filledCell = cellAbove;
                return true;
            }

            return false;
        }

        private void ResetSpawnPositionsArray()
        {
            for (int i = 0; i < _ySpawnPositions.Length; i++)
                _ySpawnPositions[i] = YMinSpawnPosition;
        }
    }
}
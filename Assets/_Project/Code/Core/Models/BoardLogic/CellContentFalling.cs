using System;
using System.Collections;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Random;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class CellContentFalling : ICellContentFalling
    {
        private const int YMinSpawnPosition = 5;
        private const float FallingSpeed = 5f;

        private readonly CellCollection _cellCollection;
        private readonly IRandomCellContentGenerator _contentGenerator;
        private readonly ICellContentMover _mover;
        private readonly int[] _ySpawnPositions = new int[Constant.Board.BoardSize.x];
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
            if (TryGetFilledCellAbove(emptyCell, out var filledCell))
            {
                _mover.MoveCellContent(
                    @from: filledCell,
                    to: emptyCell,
                    speed: FallingSpeed,
                    callback: () => onLanded(emptyCell));
                return true;
            }

            return false;
        }

        private bool TryGetFilledCellAbove(Cell emptyCell, out Cell filledCell)
        {
            filledCell = null;
            var currentCell = emptyCell;
            while (_cellCollection.TryGetCellAbove(currentCell, out var cellAbove))
            {
                if (cellAbove.Content.IsFalling || cellAbove.Content.Type == ContentType.Empty || !cellAbove.Content.Switchable)
                {
                    currentCell = cellAbove;
                    continue;
                }

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
using System.Collections;
using System.Collections.Generic;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Core.Models.Directions;
using _Project.Code.Infrastructure;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Project.Code.Core.Models
{
    public class CellContentFalling
    {
        private const int YMinSpawnPosition = 5;
        private const float FallingSpeed = 4f;
        private readonly Board _board;
        private readonly ICellContentMover _mover;
        private readonly IRandomCellContentGenerator _contentGenerator;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly int[] _ySpawnPositions = new int[9];
        private Coroutine _ySpawnPositionsResetCoroutine;

        public CellContentFalling(
            Board board, 
            ICellContentMover mover, 
            IRandomCellContentGenerator contentGenerator,
            ICoroutineRunner coroutineRunner)
        {
            _board = board;
            _mover = mover;
            _contentGenerator = contentGenerator;
            _coroutineRunner = coroutineRunner;

            ResetSpawnPositionsArray();

            _board.CellContentMatched += FillContentOnEmptyCells;
            _board.CellContentStartedMovement += FillContentOnEmptyCells;
        }

        ~CellContentFalling()
        {
            _board.CellContentStartedMovement -= FillContentOnEmptyCells;
            _board.CellContentMatched -= FillContentOnEmptyCells;
        }


        private void FillContentOnEmptyCells(Cell emptyCell) => 
            _coroutineRunner.StartCoroutine(FillContentOnEmptyCells_InNextFrame(emptyCell));

        private IEnumerator FillContentOnEmptyCells_InNextFrame(Cell emptyCell)
        {
            yield return null;
            if (!TryMoveExistingContentToEmptyCells(emptyCell)) 
                GenerateNewContentWithMovementTo(emptyCell);
        }

        private void GenerateNewContentWithMovementTo(Cell emptyCell)
        {
            if (_ySpawnPositionsResetCoroutine == null)
                _ySpawnPositionsResetCoroutine = _coroutineRunner.StartCoroutine(ResetYSpawnPosition_InNextFrame());
            
            var randomContent = _contentGenerator.Generate(
                new Vector2(emptyCell.Position.x, _ySpawnPositions[XIndexOnMatrixOfEmptyCell()]));
            
            _mover.MoveCellContent(
                contentToMove: randomContent,
                to: emptyCell,
                speed: FallingSpeed);

            _ySpawnPositions[XIndexOnMatrixOfEmptyCell()]++;

            int XIndexOnMatrixOfEmptyCell()
            {
                return Mathf.RoundToInt(emptyCell.Position.x + Constants.Board.OffsetFromCenter.x);
            }
        }

        private bool TryMoveExistingContentToEmptyCells(Cell emptyCell)
        {
            if (TryGetFilledCellAbove(emptyCell, out var filledCell))
            {
                _mover.MoveCellContent(
                    @from: filledCell,
                    to: emptyCell,
                    speed: FallingSpeed);
                return true;
            }

            return false;
        }

        private bool TryGetFilledCellAbove(Cell emptyCell, out Cell filledCell)
        {
            filledCell = null;
            var currentCell = emptyCell;
            while (_board.TryGetCellAbove(currentCell, out var cellAbove) )
            {
                if (cellAbove.Content.IsFalling)
                {
                    currentCell = cellAbove;
                    continue;
                }
                
                if (cellAbove.Content.Type != ContentType.Empty)
                {
                    filledCell = cellAbove;
                    return true;
                }

                currentCell = cellAbove;
            }

            return false;
        }

        private IEnumerator ResetYSpawnPosition_InNextFrame()
        {
            yield return null;
            ResetSpawnPositionsArray();
            _ySpawnPositionsResetCoroutine = null;
        }

        private void ResetSpawnPositionsArray()
        {
            for (int i = 0; i < _ySpawnPositions.Length; i++) 
                _ySpawnPositions[i] = YMinSpawnPosition;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Core.Models.Directions;
using _Project.Code.Infrastructure;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public class CellContentFalling
    {
        private const float FallingSpeed = 4f;
        private readonly Board _board;
        private readonly ICellContentMover _mover;
        private readonly IRandomCellContentGenerator _contentGenerator;
        private readonly ICoroutineRunner _coroutineRunner;

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

            _board.CellContentMatched += FillContentOnEmptyCells;
            _board.CellContentStartedMovement += FillContentOnEmptyCells;
        }

        ~CellContentFalling()
        {
            _board.CellContentStartedMovement -= FillContentOnEmptyCells;
            _board.CellContentMatched -= FillContentOnEmptyCells;
        }

        private void FillContentOnEmptyCells(Cell emptyCell) => 
            _coroutineRunner.StartCoroutine(FillContentOnEmptyCellsInNextFrame(emptyCell));

        private IEnumerator FillContentOnEmptyCellsInNextFrame(Cell emptyCell)
        {
            yield return null;
            if (!TryMoveExistingContentToEmptyCells(emptyCell))
            {
                // var randomContent = _contentGenerator.Generate();
                // _mover.MoveCellContent(
                //     contentToMove: randomContent,
                //     to: emptyCell,
                //     duration: FallingFromOneTileToAnotherTime);
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
    }
}
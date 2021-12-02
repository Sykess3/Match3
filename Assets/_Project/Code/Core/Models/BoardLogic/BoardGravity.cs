using System;
using System.Collections.Generic;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class BoardGravity
    {
        private readonly ICellContentFalling _contentFalling;
        private readonly IPlayerInput _playerInput;
        private bool _inFillingProcess;

        private readonly LinkedList<Cell> _cellsToFill;
        
        public Action FallingEnded;

        public BoardGravity(
            ICellContentFalling contentFalling,
            IPlayerInput playerInput)
        {
            _contentFalling = contentFalling;
            _playerInput = playerInput;

            _cellsToFill = new LinkedList<Cell>();
        }


        public void FillContentOnEmptyCell(Cell emptyCell)
        {
            _cellsToFill.AddAfter(_cellsToFill.Last, emptyCell);
            
            if (_inFillingProcess)
                return;
            
            StartFillingProcess();
        }
        
        public void FillContentOnEmptyCells(Cell[] emptyCells)
        {
            FillCellsToFill(emptyCells);
            StartFillingProcess();
        }

        private void FillCellsToFill(Cell[] emptyCells)
        {
            var previousNode = _cellsToFill.AddFirst(emptyCells[0]);

            for (int i = 1; i < emptyCells.Length; i++)
                previousNode = _cellsToFill.AddAfter(previousNode, emptyCells[i]);
        }

        private void StartFillingProcess()
        {
            _playerInput.Disable();
            _inFillingProcess = true;

            var currentNode = _cellsToFill.First;
            while (currentNode != null)
            {
                _contentFalling.FillContentOnEmptyCell(currentNode.Value, OnCellLanded);
                currentNode = currentNode.Next;
            }
        }

        private void OnCellLanded(Cell cell)
        {
            _cellsToFill.Remove(cell);
            if (_cellsToFill.Count > 0)
                return;

            _playerInput.Enable();
            FallingEnded?.Invoke();
            _cellsToFill.Clear();
            _inFillingProcess = false;
        }
    }
}
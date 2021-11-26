using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Core.Models.Directions;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Input
{
    public class BoardInputHandler : MonoBehaviour
    {
        private IPlayerInput _input;
        private Board _board;
        private Cell _selectedCell;
        private SwapCommandHandler _swapCommandHandler;
        private ICellContentSwapper _contentSwapper;

        [Inject]
        private void Construct(
            IPlayerInput input, 
            SwapCommandHandler swapCommandHandler,
            Board board,
            ICellContentSwapper contentSwapper)
        {
            _input = input;
            _board = board;
            _swapCommandHandler = swapCommandHandler;
            _contentSwapper = contentSwapper;
        }

        private void OnEnable() => _input.ClickedOnPosition += InputOnClickedOnPosition;

        private void OnDestroy() => _input.ClickedOnPosition += InputOnClickedOnPosition;

        private void InputOnClickedOnPosition(Vector2 position)
        {
            if (_board.TryGetCell(position, out var cell))
            {
                if (_selectedCell == null)
                {
                    _selectedCell = cell;
                    return;
                }

                if (IsNeighbourOfSelectedCell(cell) && _selectedCell != cell)
                {
                    _swapCommandHandler.Swap(
                        new SwapCommand(
                            firstCell: _selectedCell,
                            secondCell: cell,
                            swapper: _contentSwapper));
                    _selectedCell = null;
                    return;
                }

                _selectedCell = cell;
            }
        }

        private bool IsNeighbourOfSelectedCell(Cell cell)
        {
            var cellNeighbours= _board.GetNeighboursOf(cell);
            return cellNeighbours.Any(neighbour => neighbour == _selectedCell);
        }
    }
}
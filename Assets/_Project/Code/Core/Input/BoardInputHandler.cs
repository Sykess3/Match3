using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Swap;
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

        [Inject]
        private void Construct(
            IPlayerInput input,
            Board board)
        {
            _input = input;
            _board = board;
        }

        private void Start() => _input.ClickedOnPosition += InputOnClickedOnPosition;

        private void OnDestroy() => _input.ClickedOnPosition -= InputOnClickedOnPosition;

        private void InputOnClickedOnPosition(Vector2 position)
        {
            if (_board.TryGetCell(position, out var cell))
            {
                if (!cell.Content.Switchable)
                    return;

                if (_selectedCell == null)
                {
                    _selectedCell = cell;
                    return;
                }

                if (IsNeighbourOfSelectedCell(cell) && _selectedCell != cell && cell.Content.Switchable)
                {
                    _board.TryMatch(
                        new SwapCommand(
                            firstCell: _selectedCell,
                            secondCell: cell));
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
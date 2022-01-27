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
    public class BoardInputHandler : IInitializable, IDisposable
    {
        private readonly IPlayerInput _input;
        private readonly Board _board;
        private Cell _selectedCell;

        public event Action Swapped;

        public BoardInputHandler(Board board, IPlayerInput input)
        {
            _board = board;
            _input = input;
        }

        void IInitializable.Initialize() => _input.ClickedOnPosition += InputOnClickedOnPosition;

        void IDisposable.Dispose() => _input.ClickedOnPosition -= InputOnClickedOnPosition;

        private void InputOnClickedOnPosition(Vector2 position)
        {
            if (_board.TryGetCell(position, out var cell))
            {
                if (_selectedCell == cell)
                    return;

                cell.Select();

                if (!cell.Content.Switchable)
                    return;

                if (_selectedCell == null)
                {
                    _selectedCell = cell;
                    return;
                }

                _selectedCell.Deselect();

                if (IsNeighbourOfSelectedCell(cell) && cell.Content.Switchable)
                {
                    cell.Deselect();
                    _board.TryMatch(
                        new SwapCommand(
                            firstCell: _selectedCell,
                            secondCell: cell), OnSpawnSucceed);
                    _selectedCell = null;
                    return;
                }

                _selectedCell = cell;
            }
        }

        private void OnSpawnSucceed() => Swapped?.Invoke();

        private bool IsNeighbourOfSelectedCell(Cell cell)
        {
            var cellNeighbours = _board.GetNeighboursOf(cell);
            return cellNeighbours.Any(neighbour => neighbour == _selectedCell);
        }
    }
}
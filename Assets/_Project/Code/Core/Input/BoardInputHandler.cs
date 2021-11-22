using System;
using System.Collections.Generic;
using _Project.Code.Core.Models;
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
        private ContentSwitcher _contentSwitcher;

        [Inject]
        private void Construct(IPlayerInput input, ContentSwitcher contentSwitcher, Board board)
        {
            _input = input;
            _board = board;
            _contentSwitcher = contentSwitcher;
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

                if (IsNeighbourOfSelectedCell(cell))
                {
                    _contentSwitcher.Switch(new SwitchCommand(_selectedCell, cell));
                    _selectedCell = null;
                    return;
                }

                _selectedCell = cell;
            }
        }

        private bool IsNeighbourOfSelectedCell(Cell cell)
        {
            var eastCellPosition = _selectedCell.Position + Direction.East.GetVector2();
            var westCellPosition = _selectedCell.Position + Direction.West.GetVector2();
            var northCellPosition = _selectedCell.Position + Direction.North.GetVector2();
            var southCellPosition = _selectedCell.Position + Direction.South.GetVector2();

            if (_board.TryGetCell(eastCellPosition, out var eastCell))
                if (eastCell == cell)
                    return true;
            
            if (_board.TryGetCell(westCellPosition, out var westCell))
                if (westCell == cell)
                    return true;
            
            if (_board.TryGetCell(northCellPosition, out var northCell))
                if (northCell == cell)
                    return true;
            
            if (_board.TryGetCell(southCellPosition, out var southCell))
                if (southCell == cell)
                    return true;
            
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.Directions;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class CellCollection
    {
        private readonly Dictionary<Vector2, Cell> _cells;
        private Action<Cell> _cellContentStartedMovementCallback;

        public CellCollection(IEnumerable<Cell> cells)
        {
            _cells = cells
                .ToDictionary(x => x.Position, x => x);
        }

        public void Initialize(Action<Cell> cellContentStartedMovementCallback)
        {
            _cellContentStartedMovementCallback = cellContentStartedMovementCallback;

            SubscribeOnCellEvents();
        }

        public bool TryGetCellAbove(Cell cell, out Cell cellAbove) =>
            TryGetCell(cell.Position + Direction.North.GetVector2(), out cellAbove);

        public bool TryGetCellAbove(Vector2 position, out Cell cellAbove) =>
            TryGetCell(position + Direction.North.GetVector2(), out cellAbove);

        public bool IsStoneAbove(Vector2 position)
        {
            while (TryGetCellAbove(position, out Cell cellAbove))
            {
                if (cellAbove.Content.MatchType == DefaultContentType.Stone)
                    return true;

                position = cellAbove.Position;

            }

            return false;
        }

        public bool TryGetCellGoesDiagonallyUpwards(Vector2 position, Direction diagonalDirection, out Cell cellDiagonallyUpwards)
        {
            if (diagonalDirection == Direction.East)
                return TryGetCell(position + new Vector2(1, 1), out cellDiagonallyUpwards);

            if (diagonalDirection == Direction.West)
                return TryGetCell(position + new Vector2(-1, 1), out cellDiagonallyUpwards);

            throw new ArgumentException("Direction must be either east either west!");
        }

        public IEnumerable<Cell> GetAll(DefaultContentType ofType)
        {
            return _cells.Values.Where(TypeIsArgumentType);

            bool TypeIsArgumentType(Cell cell)
            {
                return cell.Content.MatchType == ofType;
            }
        }

        public IEnumerable<Cell> GetCellsInAllDirections(Cell relatively, DefaultContentType toEndTypeInEachDirection)
        {
            var cells = new List<Cell>(16);
            var eastCells = GetCellsInDirection(relatively, Direction.East, toEndTypeInEachDirection);
            var westCells = GetCellsInDirection(relatively, Direction.West, toEndTypeInEachDirection);
            var northCells = GetCellsInDirection(relatively, Direction.North, toEndTypeInEachDirection);
            var southCells = GetCellsInDirection(relatively, Direction.South, toEndTypeInEachDirection);

            cells.AddRange(eastCells);
            cells.AddRange(westCells);
            cells.AddRange(northCells);
            cells.AddRange(southCells);
            return cells;
        }

        public List<Cell> GetNeighboursOf(Cell cell)
        {
            var neighbours = new List<Cell>(4);

            if (TryGetCellAbove(cell, out var northCell))
                neighbours.Add(northCell);

            if (TryGetCellInDirectionRelativelyTo(cell, Direction.West, out var westCell))
                neighbours.Add(westCell);

            if (TryGetCellInDirectionRelativelyTo(cell, Direction.East, out var eastCell))
                neighbours.Add(eastCell);

            if (TryGetCellInDirectionRelativelyTo(cell, Direction.South, out var southCell))
                neighbours.Add(southCell);

            return neighbours;
        }

        public bool TryGetCell(Vector2 position, out Cell cell)
        {
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);

            return _cells.TryGetValue(new Vector2(x, y), out cell);
        }

        public IEnumerable<Cell> GetAll() =>
            _cells.Values;

        public void CleanUp()
        {
            UnsubscribeOnCellEvents();
        }

        private bool TryGetCellInDirectionRelativelyTo(Cell cell, Direction direction, out Cell cellInDirection) =>
            TryGetCell(cell.Position + direction.GetVector2(), out cellInDirection);

        private void SubscribeOnCellEvents()
        {
            foreach (var cellKvP in _cells)
                cellKvP.Value.ContentStartedMovement += OnCellContentStartedMovement;
        }

        private void UnsubscribeOnCellEvents()
        {
            foreach (var cellKvP in _cells)
                cellKvP.Value.ContentStartedMovement -= OnCellContentStartedMovement;
        }

        private IEnumerable<Cell> GetCellsInDirection(Cell relatively, Direction direction, DefaultContentType toEndType)
        {
            var current = relatively;
            while (true)
            {
                if (!TryGetCellInDirectionRelativelyTo(current, direction, out var next) ||
                    next?.Content.MatchType == toEndType)
                    yield break;

                current = next;
                yield return current;
            }
        }

        private void OnCellContentStartedMovement(object sender, EventArgs eventArgs)
        {
            var cell = (Cell) sender;
            _cellContentStartedMovementCallback?.Invoke(cell);
        }
    }
}
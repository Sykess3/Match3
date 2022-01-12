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

        public IEnumerable<Cell> GetAll(ContentType ofType)
        {
            return _cells.Values.Where(TypeIsArgumentType);

            bool TypeIsArgumentType(Cell cell)
            {
                return cell.Content.Type == ofType;
            }
        }

        public IEnumerable<Cell> GetCellsInAllDirections(Cell relatively)
        {
            var cells = new List<Cell>(16);
            var eastCells = GetCellsInDirection(relatively, Direction.East);
            var westCells = GetCellsInDirection(relatively, Direction.West);
            var northCells = GetCellsInDirection(relatively, Direction.North);
            var southCells = GetCellsInDirection(relatively, Direction.South);

            cells.AddRange(eastCells);
            cells.AddRange(westCells);
            cells.AddRange(northCells);
            cells.AddRange(southCells);
            return cells;
        }

        public IEnumerable<Cell> GetNeighboursOf(Cell cell)
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

        private IEnumerable<Cell> GetCellsInDirection(Cell relatively, Direction direction)
        {
            var current = relatively;
            while (true)
            {
                if (!TryGetCellInDirectionRelativelyTo(current, direction, out var next))
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
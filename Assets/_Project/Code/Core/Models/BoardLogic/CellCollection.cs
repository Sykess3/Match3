using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Directions;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class CellCollection
    {
        private readonly Dictionary<Vector2, Cell> _cells;
        private EventHandler _cellContentMatchedCallback;
        private Action<Cell> _cellContentStartedMovementCallback;


        public CellCollection(IEnumerable<Cell> cells)
        {
            _cells = cells
                .ToDictionary(x => x.Position, x => x);
            
        }

        public void Initialize(EventHandler cellContentMatchedCallback, Action<Cell> cellContentStartedMovementCallback)
        {
            _cellContentMatchedCallback = cellContentMatchedCallback;
            _cellContentStartedMovementCallback = cellContentStartedMovementCallback;
            
            SubscribeOnCellEvents();
        }
        
        public bool TryGetCellAbove(Cell cell, out Cell cellAbove) => 
            TryGetCell(cell.Position + Direction.South.GetVector2(), out cellAbove);
        
        public IEnumerable<Cell> GetNeighboursOf(Cell cell)
        {
            var neighbours = new List<Cell>(4);
            Vector2 eastCellPosition = cell.Position + Direction.East.GetVector2();
            Vector2 westCellPosition = cell.Position + Direction.West.GetVector2();
            Vector2 northCellPosition = cell.Position + Direction.North.GetVector2();
            Vector2 southCellPosition = cell.Position + Direction.South.GetVector2();
            
            if (TryGetCell(eastCellPosition, out var eastCell)) 
                neighbours.Add(eastCell);

            if (TryGetCell(westCellPosition, out var westCell)) 
                neighbours.Add(westCell);

            if (TryGetCell(northCellPosition, out var northCell)) 
                neighbours.Add(northCell);

            if (TryGetCell(southCellPosition, out var southCell)) 
                neighbours.Add(southCell);

            return neighbours;
        }
        
        public bool IsAnyContentMoving() => _cells.Any(ContentMoving);

        public bool TryGetCell(Vector2 position, out Cell cell)
        {
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);

            return _cells.TryGetValue(new Vector2(x, y), out cell) ;
        }

        public ReadOnlyCollection<Cell> GetAll() => 
            _cells
                .Values
                .ToList()
                .AsReadOnly();

        public void CleanUp()
        {
            UnsubscribeOnCellEvents();
        }

        private void SubscribeOnCellEvents()
        {
            foreach (var cellKvP in _cells)
            {
                cellKvP.Value.ContentMatched += OnCellContentMatched;
                cellKvP.Value.ContentStartedMovement += OnCellContentStartedMovement;
            }
        }

        private void UnsubscribeOnCellEvents()
        {
            foreach (var cellKvP in _cells)
            {
                cellKvP.Value.ContentMatched -= OnCellContentMatched;
                cellKvP.Value.ContentStartedMovement -= OnCellContentStartedMovement;
            }
        }

        private void OnCellContentStartedMovement(object sender, EventArgs eventArgs)
        {
            var cell = (Cell) sender;
            _cellContentStartedMovementCallback?.Invoke(cell);
        }

        private static bool ContentMoving(KeyValuePair<Vector2, Cell> x) => 
            x.Value.Content.IsFalling;

        private void OnCellContentMatched(object sender, EventArgs e)
        {
            var cell = (Cell) sender;
            _cellContentMatchedCallback?.Invoke(cell, e);
        }
    }
}
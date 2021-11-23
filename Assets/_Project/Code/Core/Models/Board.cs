using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure.Factories;
using _Project.Code.Infrastructure.Installers.Factories;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public class Board : IModel
    {
        private readonly IBoardConfig _config;
        private readonly Dictionary<Vector2, Cell> _cells;

        public Board(IEnumerable<Cell> cells, IBoardConfig config)
        {
            _config = config;
            _cells = cells
                .ToDictionary(x => x.Position, y => y);
        }

        public bool TryGetCell(Vector2 position, out Cell cell)
        {
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);

            var tryGetValue = _cells.TryGetValue(new Vector2(x, y), out cell);
            return tryGetValue && cell.Filler != null; //TODO: Remove null check
        }
    }
}
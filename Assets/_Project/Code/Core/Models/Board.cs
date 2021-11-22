using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure.Installers.Factories;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public class Board : IModel
    {
        private readonly IBoardConfig _config;
        private readonly CellContentFactory _contentFactory;
        private readonly Dictionary<Vector2, Cell> _cells;

        public Board(IEnumerable<Cell> cells, IBoardConfig config, CellContentFactory contentFactory)
        {
            _config = config;
            _contentFactory = contentFactory;
            _cells = cells
                .ToDictionary(x => x.Position, y => y);
        }

        public void FillCells()
        {
            foreach (var cellKvP in _cells)
            {
                cellKvP.Value.Filler = _contentFactory.Create(Cell.ContentType.Red);
                cellKvP.Value.Filler.Position = cellKvP.Value.Position;
            }
        }
        public bool TryGetCell(Vector2 position, out Cell cell)
        {
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);

            var tryGetValue = _cells.TryGetValue(new Vector2(x, y), out cell);
            return cell != null; //TODO: Remove null check
        }
    }
}
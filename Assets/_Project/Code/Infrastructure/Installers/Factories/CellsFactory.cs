using System.Collections.Generic;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Factories
{
    public class CellsFactory : IFactory<IEnumerable<Cell>>
    {
        private readonly IBoardConfig _boardConfig;

        public CellsFactory(IBoardConfig boardConfig)
        {
            _boardConfig = boardConfig;
        }
        
        public IEnumerable<Cell> Create()
        {
            var size = _boardConfig.Size;
            var cells = new Cell[size.x * size.y];

            Vector2 offset = new Vector2((size.x - 1)
                                         * 0.5f, (size.y - 1) * 0.5f);
            for (int i = 0, index = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++, index++)
                {
                    var position = new Vector2(x: i - offset.x, y: j - offset.y);
                    var cell = new Cell(position);
                    cells[index] = cell;
                }
            }

            return cells;
        }
    }
}
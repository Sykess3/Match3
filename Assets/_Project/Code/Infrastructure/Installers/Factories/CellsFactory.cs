using System.Collections.Generic;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Factories
{
    public class CellsFactory : IFactory<IEnumerable<Cell>>
    {
        private readonly IBoardConfig _boardConfig;
        private readonly IRandomCellContentGenerator _randomCellContentGenerator;

        public CellsFactory(IBoardConfig boardConfig,
            IRandomCellContentGenerator randomCellContentGenerator)
        {
            _boardConfig = boardConfig;
            _randomCellContentGenerator = randomCellContentGenerator;
        }

        public IEnumerable<Cell> Create()
        {   
            Constants.Board.InitializeOffset(_boardConfig.Size);

            var cells = CreateCellsWithInitializationOfContent(Constants.Board.OffsetFromCenter);
            return cells;
        }

        private Cell[] CreateCellsWithInitializationOfContent(Vector2 offset)
        {
            var size = _boardConfig.Size;
            var cells = new Cell[size.x * size.y];
            for (int i = 0, index = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++, index++)
                {
                    var position = new Vector2(x: i - offset.x, y: j - offset.y);
                    var cell = new Cell(position)
                    {
                        Content = _randomCellContentGenerator.Generate(position)
                    };
                    cells[index] = cell;
                }
            }

            return cells;
        }
    }
}
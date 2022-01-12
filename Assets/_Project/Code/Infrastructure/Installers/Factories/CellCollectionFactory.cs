using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Random;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Factories
{
    public class CellCollectionFactory : IFactory<CellCollection>
    {
        private readonly IRandomCellContentGenerator _randomCellContentGenerator;

        public CellCollectionFactory(IRandomCellContentGenerator randomCellContentGenerator)
        {
            _randomCellContentGenerator = randomCellContentGenerator;
        }

        /// <summary>
        /// Fill Content by random content with no matches
        /// </summary>
        public CellCollection Create()
        {
            var size = Constant.Board.Size;
            var offset = Constant.Board.OffsetFromCenter;
            
            Cell[] cells = CreateCellsWithRandomGeneratedCellContent(size, offset);
            
            return new CellCollection(cells);
        }

        private Cell[] CreateCellsWithRandomGeneratedCellContent(Vector2Int size, Vector2 offset)
        {
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
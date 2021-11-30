using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Models.Random;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class CellCollectionFactory
    {
        private readonly IRandomCellContentGenerator _randomCellContentGenerator;

        public CellCollectionFactory(IRandomCellContentGenerator randomCellContentGenerator)
        {
            _randomCellContentGenerator = randomCellContentGenerator;
        }

        /// <summary>
        /// Fill Content by random content with no matches
        /// </summary>
        public CellCollection CreateWithFilling()
        {
            var size = Constants.Board.BoardSize;
            var offset = Constants.Board.OffsetFromCenter;
            
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
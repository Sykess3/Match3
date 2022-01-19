using System.ComponentModel;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Models.Random;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Factories
{
    public class CellCollectionFactory : IFactory<CellCollection>
    {
        private readonly IRandomCellContentGenerator _randomCellContentGenerator;
        private readonly ILevelConfig _levelConfig;

        private readonly ICellContentFactory _cellContentFactory;
        private readonly IContentDecorator _decorator;

        public CellCollectionFactory(
            IRandomCellContentGenerator randomCellContentGenerator,
            ILevelConfig levelConfig,
            ICellContentFactory cellContentFactory,
            IContentDecorator decorator)
        {
            _levelConfig = levelConfig;
            _cellContentFactory = cellContentFactory;
            _decorator = decorator;
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

                    var cell = new Cell(position);
                    if (_levelConfig.IsStone(position))
                    {
                        cell.Content = _cellContentFactory.Create(ContentType.Stone);
                        cell.Content.Position = position;
                    }
                    else
                    {
                        cell.Content = GetRandomCellContent(position);
                    }

                    cells[index] = cell;
                }
            }

            return cells;
        }

        private CellContentBase GetRandomCellContent(Vector2 position)
        {
            var cellContent = _randomCellContentGenerator.Generate(position);
            var decoratorType = _levelConfig.GetDecorator(position);
            CellContentBase decoratedContentBase =
                _decorator.Decorate(contentBaseToDecorate: cellContent, type: decoratorType);
            return decoratedContentBase;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.Pool;
using _Project.Code.Core.Models.Directions;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Models.Random;
using _Project.Code.Infrastructure.Factories;
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

            var cellCollection = new CellCollection(cells);

            return cellCollection;
        }

        private Cell[] CreateCellsWithRandomGeneratedCellContent(Vector2Int size, Vector2 offset)
        {
            var cells = new Cell[size.x * size.y];
            Tuple<Cell, Cell> southNeighbours = null;
            Tuple<Cell, Cell> westNeighbours = null;
                
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
                        NewMethod(i, cells, j, cell, position);
                    }

                    cells[index] = cell;
                }
            }

            return cells;
        }

        private void NewMethod(int i, Cell[] cells, int j, Cell cell, Vector2 position)
        {
            Tuple<Cell, Cell> westNeighbours;
            Tuple<Cell, Cell> southNeighbours;
            if (i < 2)
            {
                westNeighbours = null;
            }
            else
            {
                var previousWest = cells.Single(x => x?.Position == new Vector2(position.x - 1, position.y));
                var beforePreviousWest = cells.Single(x => x?.Position == new Vector2(position.x - 2, position.y));

                westNeighbours = new Tuple<Cell, Cell>(previousWest, beforePreviousWest);
            }

            if (j < 2)
            {
                southNeighbours = null;
            }
            else
            {
                var previousSouth = cells.Single(x => x?.Position == new Vector2(position.x, position.y - 1));
                var beforePreviousSouth = cells.Single(x => x?.Position == new Vector2(position.x, position.y - 2));

                southNeighbours = new Tuple<Cell, Cell>(previousSouth, beforePreviousSouth);
            }

            cell.Content = GetRandomCellContent(position, southNeighbours, westNeighbours);
        }

        private CellContentBase GetRandomCellContent(Vector2 position, Tuple<Cell, Cell> southNeighbours,
            Tuple<Cell, Cell> westNeighbours)
        {
            var cellContent = _randomCellContentGenerator.GenerateUnmatchable(position, southNeighbours, westNeighbours);
            var decoratorType = _levelConfig.GetDecorator(position);
            CellContentBase decoratedContentBase =
                _decorator.Decorate(contentBaseToDecorate: cellContent, type: decoratorType);
            return decoratedContentBase;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.Pool;
using _Project.Code.Core.Models.Directions;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Models.Random
{
    public class RandomCellContentGenerator : IRandomCellContentGenerator
    {
        private readonly ICellContentPool _cellContentPool;
        private readonly Dictionary<ContentType, float> _contentToSpawn;

        public RandomCellContentGenerator(ICellContentPool cellContentPool, ILevelConfig levelConfig)
        {
            _cellContentPool = cellContentPool;
            _contentToSpawn = levelConfig.ContentToSpawnTypeChanceMap;
        }

        public CellContentBase Generate(Vector2 position)
        {
            var range = UnityEngine.Random.Range(0f, 1f);
            foreach (var contentKvP in _contentToSpawn)
            {
                if (contentKvP.Value >= range)
                {
                    CellContentBase cellContentBase = _cellContentPool.Get(contentKvP.Key);
                    cellContentBase.Position = position;
                    return cellContentBase;
                }
            }

            throw new InvalidOperationException("ILevelConfig implementation is not having a max chance(1) or " +
                                                "contentToSpawn is empty");
        }

        public CellContentBase GenerateUnmatchable(Vector2 position, Tuple<Cell, Cell> southNeighbours,
            Tuple<Cell, Cell> westNeighbours)
        {
            var range = UnityEngine.Random.Range(0f, 1f);
            foreach (var contentKvP in _contentToSpawn)
            {
                if (contentKvP.Value >= range)
                {
                    var contentType = GetUnmatchable(contentKvP.Key, southNeighbours, westNeighbours);
                    CellContentBase cellContentBase = _cellContentPool.Get(contentType);
                    cellContentBase.Position = position;
                    return cellContentBase;
                }
            }

            throw new InvalidOperationException("ILevelConfig implementation is not having a max chance(1) or " +
                                                "contentToSpawn is empty");
        }

        private ContentType GetUnmatchable(ContentType contentType, Tuple<Cell, Cell> southNeighbours,
            Tuple<Cell, Cell> westNeighbours)
        {
            while (IsAnyMatches(contentType, southNeighbours) || IsAnyMatches(contentType, westNeighbours))
                contentType = contentType.GetAnother();

            return contentType;
        }

        private bool IsAnyMatches(ContentType contentType, Tuple<Cell, Cell> neighbours)
        {
            if (neighbours != null)
            {
                bool isSouthNeighboursSimilar = IsAnyMatches(neighbours.Item1.Content.MatchableContent,
                    neighbours.Item2.Content.MatchableContent);
                if (isSouthNeighboursSimilar)
                    return IsAnyMatches(contentType, neighbours.Item1.Content.MatchableContent);
            }

            return false;
        }

        private bool IsAnyMatches(ContentType single, IEnumerable<ContentType> contentMatchableContent) =>
            contentMatchableContent.Any(contentType => single == contentType);

        private bool IsAnyMatches(IEnumerable<ContentType> matchableContent1,
            IEnumerable<ContentType> matchableContent2)
        {
            foreach (var content1 in matchableContent1)
            foreach (var content2 in matchableContent2)
                if (content1 == content2)
                    return true;

            return false;
        }
    }
}
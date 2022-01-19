using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.Pool;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

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
            CellContentBase cellContentBase = null;
            foreach (var contentKvP in _contentToSpawn)
            {
                if (contentKvP.Value >= range)
                {
                    cellContentBase = _cellContentPool.Get(contentKvP.Key);
                    cellContentBase.Position = position;
                    return cellContentBase;
                }
            }

            throw new InvalidOperationException("ILevelConfig implementation is not having a max chance(1) or " +
                                                "contentToSpawn is empty");
        }
    }
}
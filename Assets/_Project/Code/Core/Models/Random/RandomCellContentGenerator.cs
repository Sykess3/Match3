using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Models.Random
{
    public class RandomCellContentGenerator : IRandomCellContentGenerator
    {
        private readonly ICellContentFactory _cellContentFactory;
        private readonly Dictionary<ContentType, float> _contentToSpawn;

        public RandomCellContentGenerator(ICellContentFactory cellContentFactory, ILevelConfig levelConfig)
        {
            _cellContentFactory = cellContentFactory;
            _contentToSpawn = levelConfig.ContentToSpawn;
        }

        public CellContent Generate(Vector2 position)
        {
            var range = UnityEngine.Random.Range(0f, 1f);
            foreach (var contentKvP in _contentToSpawn)
            {
                if (contentKvP.Value >= range) 
                    return _cellContentFactory.Create(contentKvP.Key, position);
            }

            throw new InvalidOperationException("ILevelConfig implementation is not having a max chance(1) or " +
                                                "contentToSpawn is empty");
        }
    }
}
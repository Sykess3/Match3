﻿using System;
using System.Collections.Generic;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure.Installers.Factories;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Code.Core.Models
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
            var range = Random.Range(0f, 1f);
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
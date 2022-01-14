using System;
using System.Collections.Generic;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Level/LevelDecorators")]
    public class LevelDecoratorsConfigs : ScriptableObject
    {
        public Dictionary<DecoratorType, ICellContentConfig> DecoratorConfigs { get; set; } =
            new Dictionary<DecoratorType, ICellContentConfig>();
    }
}
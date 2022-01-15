using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Level/LevelDecorators")]
    public class LevelDecoratorsConfigs : ScriptableObject
    {
        [SerializeField] private ContentDecoratorConfig[] _configs;

        public IEnumerable<IContentDecoratorConfig> DecoratorConfigs => _configs;
    }
}
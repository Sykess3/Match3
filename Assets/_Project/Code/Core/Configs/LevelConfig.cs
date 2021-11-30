using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Level")]
    public class LevelConfig : ScriptableObject, ILevelConfig
    {
        [SerializeField] private Pair[] _contentToSpawn;

        public Dictionary<ContentType, float> ContentToSpawn =>
            _contentToSpawn
                .OrderBy(x => x.ChanceToSpawn.Min)
                .ToDictionary(x => x.Type, x => x.ChanceToSpawn.Max);
        
        [Serializable]
        class Pair
        {
            public ContentType Type;
            [FloatRangeSlider(0,1)]
            public FloatRange ChanceToSpawn;
        }    
    }
    
}
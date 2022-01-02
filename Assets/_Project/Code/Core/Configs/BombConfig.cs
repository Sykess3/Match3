using System;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/CellContent/Bomb", order = 0)]
    public class BombConfig : CellContentConfig, IBombConfig
    {
        [SerializeField] private IntRange _amountOfContentToDestroy;

        public int MaxContentAmountToDestroy => _amountOfContentToDestroy.Max;
        public int MinContentAmountToDestroy => _amountOfContentToDestroy.Min;
    }

    [Serializable]
    public class IntRange
    {
        [SerializeField] private int _min;
        [SerializeField] private int _max;
        
        public int Max => _max;
        public int Min => _min;
    }
}
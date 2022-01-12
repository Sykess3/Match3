using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Level")]
    public class LevelConfig : ScriptableObject, ILevelConfig
    {
        [SerializeField] private Pair[] _contentToSpawn;
        [SerializeField] private ParticlesPair[] _particles;

        [SerializeField] private CellContentEditorSettings[] _contentEditorSettings = 
            new CellContentEditorSettings[Constant.Board.Size.x * Constant.Board.Size.y];
        
        public CellContentEditorSettings[] ContentEditorSettings
        {
            get => _contentEditorSettings;
            set => _contentEditorSettings = value;
        }

        public Dictionary<ContentType, ParticleSystem> Particles
        {
            get => GetSerializedParticlesConfig();
            set => SerializeParticlesConfigs(value);
        }

        public Dictionary<ContentType, float> ContentToSpawnTypeChanceMap =>
            _contentToSpawn
                .OrderBy(x => x.ChanceToSpawn.Min)
                .ToDictionary(x => x.Type, x => x.ChanceToSpawn.Max);

        public DecoratorType GetDecorator(Vector2 position)
        {
            return _contentEditorSettings.Single(PositionsIsEqual).DecoratorType;

            bool PositionsIsEqual(CellContentEditorSettings x)
            {
                return x.Position == position;
            }
        }

        public bool IsStone(Vector2 position)
        {
            return _contentEditorSettings.Single(PositionsIsEqual).IsStone;
            
            bool PositionsIsEqual(CellContentEditorSettings x)
            {
                return x.Position == position;
            }
        }

        [ContextMenu("Reset positions")]
        public void RecalculateBoardEditorSettingsPositions()
        {
            for (int i = 0, index = 0; i < Constant.Board.Size.x; i++)
            {
                for (int j = 0; j < Constant.Board.Size.y; j++, index++)
                {
                    var position = new Vector2(x: i - Constant.Board.OffsetFromCenter.x,
                        y: j - Constant.Board.OffsetFromCenter.y);
                    _contentEditorSettings[index].Position = position;
                }
            }
        }

        private void SerializeParticlesConfigs(Dictionary<ContentType, ParticleSystem> value)
        {
            _particles = new ParticlesPair[value.Count];
            int index = 0;
            foreach (var patricles in value)
            {
                _particles[index] = new ParticlesPair
                {
                    Particle = patricles.Value,
                    Type = patricles.Key
                };
                index++;
            }
        }

        private Dictionary<ContentType, ParticleSystem> GetSerializedParticlesConfig() =>
            _particles.ToDictionary(x => x.Type, x => x.Particle);

        [Serializable]
        class Pair
        {
            public ContentType Type;
            [FloatRangeSlider(0, 1)] public FloatRange ChanceToSpawn;
        }

        [Serializable]
        class ParticlesPair
        {
            public ContentType Type;
            public ParticleSystem Particle;
        }

        [Serializable]
        public class CellContentEditorSettings
        {
            public Vector2 Position;
            public bool IsStone;
            public DecoratorType DecoratorType;
        }
    }
    
}
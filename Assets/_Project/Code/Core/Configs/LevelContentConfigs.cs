using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Level/ContentConfigs")]
    public class LevelContentConfigs : ScriptableObject
    {
        [SerializeField] private ContentToSpawnPair[] _contentToSpawn;
        [SerializeField] private ParticlesPair[] _particles;
        [SerializeField] private CellContentConfig[] _contentConfigs;

        public Dictionary<DefaultContentType, float> ContentToSpawnTypeChanceMap =>
            _contentToSpawn
                .OrderBy(x => x.ChanceToSpawn.Min)
                .ToDictionary(x => x.Type, x => x.ChanceToSpawn.Max);

        public Dictionary<DefaultContentType, ParticleSystem> Particles
        {
            get => GetSerializedInEditor_ParticlesConfig();
            set => SerializeForEditor_ParticlesConfigs(value);
        }

        public CellContentConfig[] ContentConfigs
        {
            get => _contentConfigs;
            set => _contentConfigs = value;
        }
        
        public void SetContentToSpawn_Editor(ContentToSpawnPair[] contentToSpawn) => _contentToSpawn = contentToSpawn;

        private void SerializeForEditor_ParticlesConfigs(Dictionary<DefaultContentType, ParticleSystem> value)
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

        private Dictionary<DefaultContentType, ParticleSystem> GetSerializedInEditor_ParticlesConfig() =>
            _particles.ToDictionary(x => x.Type, x => x.Particle);
        
        [Serializable]
        public class ContentToSpawnPair
        {
            public DefaultContentType Type;
            [FloatRangeSlider(0, 1)] public FloatRange ChanceToSpawn;
        }

        [Serializable]
        class ParticlesPair
        {
            public DefaultContentType Type;
            public ParticleSystem Particle;
        }

        [Serializable]
        public class CellContentPair
        {
            public DefaultContentType Type;
            public CellContentConfig CellContentConfig;
        }
    }
}
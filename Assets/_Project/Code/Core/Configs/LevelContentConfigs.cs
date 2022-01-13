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

        public Dictionary<ContentType, float> ContentToSpawnTypeChanceMap =>
            _contentToSpawn
                .OrderBy(x => x.ChanceToSpawn.Min)
                .ToDictionary(x => x.Type, x => x.ChanceToSpawn.Max);

        public Dictionary<ContentType, ParticleSystem> Particles
        {
            get => GetSerializedParticlesConfig();
            set => SerializeParticlesConfigs(value);
        }

        public void SetContentToSpawn_Editor(ContentToSpawnPair[] contentToSpawn) => _contentToSpawn = contentToSpawn;

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
        public class ContentToSpawnPair
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
    }
}
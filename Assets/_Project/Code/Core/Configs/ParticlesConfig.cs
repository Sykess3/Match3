using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Particles")]
    public class ParticlesConfig : ScriptableObject, IParticlesConfig
    {
        [SerializeField] private Pair[] _particles;

        public Dictionary<DefaultContentType, ParticleSystem> Particles =>
            _particles.ToDictionary(x => x.defaultContentType, x => x.Particle);

        [Serializable]
        private class Pair
        {
            [FormerlySerializedAs("ContentType")] public DefaultContentType defaultContentType;
            public ParticleSystem Particle;
        }
    }
}
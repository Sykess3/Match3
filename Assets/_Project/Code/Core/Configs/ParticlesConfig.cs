using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Particles")]
    public class ParticlesConfig : ScriptableObject, IParticlesConfig
    {
        [SerializeField] private Pair[] _particles;

        public Dictionary<ContentType, ParticleSystem> Particles =>
            _particles.ToDictionary(x => x.ContentType, x => x.Particle);

        [Serializable]
        private class Pair
        {
            public ContentType ContentType;
            public ParticleSystem Particle;
        }
    }
}
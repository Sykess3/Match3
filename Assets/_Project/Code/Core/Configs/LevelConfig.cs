﻿using System;
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
        [SerializeField] private ParticlesPair[] _particles;

        private Dictionary<ContentType, ParticleSystem> _particlesKvP;

        public Dictionary<ContentType, ParticleSystem> Particles
        {
            get => _particlesKvP;
            set
            {
                _particlesKvP = value;
                FillParticlesViewForEditor(value);
            }
        }

        public Dictionary<ContentType, float> ContentToSpawnTypeChanceMap =>
            _contentToSpawn
                .OrderBy(x => x.ChanceToSpawn.Min)
                .ToDictionary(x => x.Type, x => x.ChanceToSpawn.Max);

        private void FillParticlesViewForEditor(Dictionary<ContentType, ParticleSystem> value)
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
    }
}
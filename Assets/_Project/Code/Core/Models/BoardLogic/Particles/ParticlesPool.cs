using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.DataStructures;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure.Loading;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Particles
{
    public class ParticlesPool : ObjectPool<DefaultContentType, ParticlesWrapper> , IParticlesPool
    {
        public ParticlesPool(IAssetProvider assetProvider, ILevelConfig config) 
            : base(new Factory(assetProvider, config))
        {
            
        }

        private class Factory : IObjectPoolFactory<DefaultContentType, ParticlesWrapper>
        {
            private readonly IAssetProvider _assetProvider;
            private readonly Dictionary<DefaultContentType, ParticleSystem> _particles;
            private readonly Transform _parent;

            public Factory(IAssetProvider assetProvider, ILevelConfig levelConfig)
            {
                _assetProvider = assetProvider;
                _particles = levelConfig.Particles;
                
                _parent = new GameObject("Particles").transform;
            }

            public ParticlesWrapper Create(DefaultContentType type)
            {
                var particlePrefab = _particles[type];
                var particleObject = _assetProvider.Instantiate<ParticleSystem>(particlePrefab.gameObject, _parent);
                return new ParticlesWrapper(particleObject, type);
            }
        } 
    }
}
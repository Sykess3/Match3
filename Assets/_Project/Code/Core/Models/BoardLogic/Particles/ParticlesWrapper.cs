using System;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.DataStructures;
using _Project.Code.Core.Views.Particles;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Particles
{
    public class ParticlesWrapper : IPoolItem<ContentType>
    {
        private readonly ParticleSystem _particles;

        public event EventHandler Disabled;

        public ContentType MatchType { get; }

        public Vector2 Position
        {
            get => _particles.transform.position;
            set => _particles.transform.position = value;
        }

        public ParticlesWrapper(ParticleSystem particles, ContentType type)
        {
            MatchType = type;
            _particles = particles;
            if (!particles.gameObject.TryGetComponent(out ParticlesDisabledEvent @event))
                throw new ArgumentException("Add ParticlesDisabledEvent to ParticleSystem prefab");

            @event.OnStopped += Disable;
        }

        private void Disable() => Disabled?.Invoke(this, EventArgs.Empty);

        public void Enable() => _particles.Play();
    }
}
using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using UnityEngine;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface IParticlesConfig
    {
        Dictionary<ContentType, ParticleSystem> Particles { get; }
    }
}
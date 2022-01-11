using System.Collections.Generic;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models.BoardLogic.Cells;
using UnityEngine;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface ILevelConfig
    {
        Dictionary<ContentType, float> ContentToSpawnTypeChanceMap { get; }
        Dictionary<ContentType, ParticleSystem> Particles { get; }
    }
}
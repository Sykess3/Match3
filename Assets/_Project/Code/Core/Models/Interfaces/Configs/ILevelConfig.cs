using System.Collections.Generic;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEngine;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface ILevelConfig
    {
        Dictionary<DefaultContentType, float> ContentToSpawnTypeChanceMap { get; }
        Dictionary<DefaultContentType, ParticleSystem> Particles { get; }
        ILevelGoalConfig LevelGoal { get; }
        int MaxStepsCount { get; }
        DecoratorType GetDecorator(Vector2 position);
        bool IsStone(Vector2 position);
    }
}
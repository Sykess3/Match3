using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Meta.Views;
using _Project.Code.Meta.Views.Hud;
using UnityEngine;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface ILevelGoalConfig
    {
        SingleGoalView Prefab { get; }
        Dictionary<DefaultContentType, int> DefaultGoal { get; }
        Dictionary<DecoratorType, int> DecoratorsGoal { get; }
        Sprite[] DefaultContentImages { get; }
        Sprite[] DecoratorContentImages { get; }
    }
}
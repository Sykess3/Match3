using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEngine;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface IContentDecoratorConfig
    {
        DecoratorType Type { get; }
        bool Switchable { get; }
        GameObject Prefab { get; } 
    }
}
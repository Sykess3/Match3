using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Views;
using UnityEngine;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface ICellContentConfig
    {
        ContentType ContentType { get; }
        IEnumerable<ContentType> MatchableContent { get; }
        bool Switchable { get; }
        GameObject Prefab { get; } 
    }
}
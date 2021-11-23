using System.Collections.Generic;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models.Cells;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface ILevelConfig
    {
        Dictionary<Cell.ContentType, float> ContentToSpawn { get; }
    }
}
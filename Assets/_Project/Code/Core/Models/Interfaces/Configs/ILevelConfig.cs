using System.Collections.Generic;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models.Cells;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface ILevelConfig
    {
        Dictionary<ContentType, float> ContentToSpawn { get; }
    }
}
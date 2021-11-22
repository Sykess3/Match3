using System.Collections.Generic;

namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface ICellContentConfig
    {
        Cell.ContentType ContentType { get; }
        IEnumerable<Cell.ContentType> MatchableContent { get; }
        bool Switchable { get; }
    }
}
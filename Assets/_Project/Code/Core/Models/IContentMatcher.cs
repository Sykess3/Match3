using System.Collections.Generic;

namespace _Project.Code.Core.Models
{
    public interface IContentMatcher
    {
        IReadOnlyCollection<Cell> Match(Cell commandCell1);
    }
}
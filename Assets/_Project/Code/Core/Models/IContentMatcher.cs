using System.Collections.Generic;
using _Project.Code.Core.Models.Cells;

namespace _Project.Code.Core.Models
{
    public interface IContentMatcher
    {
        List<Cell> Match(Cell commandCell1);
    }
}
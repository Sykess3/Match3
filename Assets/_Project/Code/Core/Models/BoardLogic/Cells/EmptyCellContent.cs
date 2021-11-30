using System.Collections.Generic;
using System.Linq;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    public class EmptyCellContent : CellContent
    {
        public override ContentType Type => ContentType.Empty;
        public override bool Switchable => false;
        public override IEnumerable<ContentType> MatchableContent => Enumerable.Empty<ContentType>();

        public EmptyCellContent() 
            : base(null)
        {
            
        }
    }
}
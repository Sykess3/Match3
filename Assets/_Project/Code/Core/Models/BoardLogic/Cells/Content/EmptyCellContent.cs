using System.Collections.Generic;
using System.Linq;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public class EmptyCellContent : CellContent
    {
        public static EmptyCellContent GetCached { get; } = new EmptyCellContent();
        public override ContentType Type => ContentType.Empty;
        public override bool Switchable => false;
        public override IEnumerable<ContentType> MatchableContent => Enumerable.Empty<ContentType>();

        public EmptyCellContent() 
            : base(null)
        {
            
        }
    }
}
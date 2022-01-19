using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.DataStructures;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public class CellContent : CellContentBase, IPoolItem<ContentType>
    {
        public event Action Enabled;
        public CellContent(IEnumerable<ContentType> matchableContent, ContentType matchType, bool switchable) : base(matchableContent, matchType, switchable)
        {
        }

        protected override CellContentBase GetDecorator() => GetEmptyCached;
        

        public void Enable() => Enabled?.Invoke();
    }
}
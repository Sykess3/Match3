using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.DataStructures;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public class DefaultCellContent : CellContentBase, IPoolItem<DefaultContentType>
    {
        public event Action Enabled;
        public DefaultCellContent(IEnumerable<DefaultContentType> matchableContent, DefaultContentType matchType, bool switchable) 
            : base(matchableContent, matchType, switchable, DecoratorType.None)
        {
        }

        protected override CellContentBase GetDecorator() => GetEmptyCached;
        

        public void Enable() => Enabled?.Invoke();
    }
}
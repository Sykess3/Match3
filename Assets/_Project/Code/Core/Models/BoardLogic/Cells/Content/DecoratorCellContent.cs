using System;
using _Project.Code.Core.Models.Interfaces.Configs;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public class DecoratorCellContent : CellContentBase
    {
        private readonly CellContentBase _decorateTarget;

        public DecoratorCellContent(CellContentBase decorateTarget, DecoratorType decoratorType, bool switchable)
            : base(decorateTarget.MatchableContent, decorateTarget.MatchType,
                switchable, decoratorType)
        {
            _decorateTarget = decorateTarget;
            Position = decorateTarget.Position;
        }

        protected override CellContentBase GetDecorator() => _decorateTarget;
    }
}
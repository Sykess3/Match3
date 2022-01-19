using System;
using _Project.Code.Core.Models.Interfaces.Configs;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public class Decorator_CellContent : CellContentBase
    {
        private readonly CellContentBase _decorateTarget;

        public Decorator_CellContent(CellContentBase decorateTarget, bool switchable)
            : base(decorateTarget.MatchableContent, decorateTarget.MatchType,
                switchable)
        {
            _decorateTarget = decorateTarget;
            Position = decorateTarget.Position;
        }

        protected override CellContentBase GetDecorator() => _decorateTarget;
    }
}
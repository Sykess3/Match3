using _Project.Code.Core.Models.Interfaces.Configs;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public class CellContentDecorator : CellContent
    {
        private readonly CellContent _decorateTarget;

        public CellContentDecorator(ICellContentConfig config, CellContent decorateTarget) : base(config)
        {
            _decorateTarget = decorateTarget;
        }

        protected override CellContent GetDecorator() => _decorateTarget;
    }
}
using _Project.Code.Core.Models.Interfaces.Configs;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public class CellContentDecorator : CellContent
    {
        private readonly CellContent _decorateTarget;
        private readonly IContentDecoratorConfig _contentDecoratorConfig;

        public CellContentDecorator(CellContent decorateTarget, IContentDecoratorConfig contentDecoratorConfig) : base(decorateTarget.Config)
        {
            _decorateTarget = decorateTarget;
            _contentDecoratorConfig = contentDecoratorConfig;
            Position = decorateTarget.Position;
        }

        protected override CellContent GetDecorator() => _decorateTarget;
        public override bool Switchable => _contentDecoratorConfig.Switchable;
    }
}
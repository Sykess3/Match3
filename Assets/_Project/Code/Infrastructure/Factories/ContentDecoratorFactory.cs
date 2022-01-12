using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;

namespace _Project.Code.Infrastructure.Factories
{
    public class ContentDecoratorFactory : IContentDecoratorsFactory
    {
        private readonly Dictionary<DecoratorType, ICellContentConfig> _decoratorsConfigs;

        public ContentDecoratorFactory(Dictionary<DecoratorType, ICellContentConfig> decoratorsConfigs)
        {
            _decoratorsConfigs = decoratorsConfigs;
        }

        public CellContent Decorate(CellContent contentToDecorate, DecoratorType type)
        {
            var config = _decoratorsConfigs[type];

            int packCount = type.PackCount();
            var previousContent = new CellContentDecorator(config ,contentToDecorate);
            for (int i = 0; i < packCount - 1; i++)
            {
                previousContent = new CellContentDecorator(config, previousContent);
            }

            return previousContent;
        }
    }
}
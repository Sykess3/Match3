using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Services;
using UnityEngine;

namespace _Project.Code.Infrastructure.Factories
{
    public class ContentDecoratorFactory : IContentDecoratorsFactory
    {
        private readonly IAssetProvider _provider;
        private readonly Dictionary<DecoratorType, IContentDecoratorConfig> _decoratorConfigs;
        private Transform _parent;

        public ContentDecoratorFactory(IEnumerable<IContentDecoratorConfig> decoratorConfigs, IAssetProvider provider)
        {
            _provider = provider;
            _decoratorConfigs = decoratorConfigs.ToDictionary(x => x.Type, x => x);
            _parent = new GameObject("Decorators").transform;
        }

        public CellContent Decorate(CellContent contentToDecorate, DecoratorType type)
        {
            if (type == DecoratorType.None)
                return contentToDecorate;
            
            var config = _decoratorConfigs[type];
            
            var decoratedModel = DecorateModel(contentToDecorate, type, config);
            var view = _provider.Instantiate<CellContentView>(config.Prefab, _parent);
            var contentDecoratorPresenter = new ContentDecoratorPresenter(decoratedModel, view);
            
            return decoratedModel;
        }

        private static CellContentDecorator DecorateModel(CellContent contentToDecorate, DecoratorType type,
            IContentDecoratorConfig config)
        {
            int packCount = type.PackCount();
            var previousContent = new CellContentDecorator(contentToDecorate, config);
            for (int i = 0; i < packCount - 1; i++)
                previousContent = new CellContentDecorator(previousContent, config);
            return previousContent;
        }
    }
}
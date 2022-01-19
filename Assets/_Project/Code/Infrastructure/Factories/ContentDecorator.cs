using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Services;
using UnityEngine;

namespace _Project.Code.Infrastructure.Factories
{
    public class ContentDecorator : IContentDecorator
    {
        private readonly IAssetProvider _provider;
        private readonly Dictionary<DecoratorType, IContentDecoratorConfig> _decoratorConfigs;
        private readonly Transform _parent;

        public ContentDecorator(IEnumerable<IContentDecoratorConfig> decoratorConfigs, IAssetProvider provider)
        {
            _provider = provider;
            _decoratorConfigs = decoratorConfigs.ToDictionary(x => x.Type, x => x);
            _parent = new GameObject("Decorators").transform;
        }

        public CellContentBase Decorate(CellContentBase contentBaseToDecorate, DecoratorType type)
        {
            if (type == DecoratorType.None)
                return contentBaseToDecorate;

            var config = _decoratorConfigs[type];

            var view = _provider.Instantiate<Decorator_CellContentView>(config.Prefab, _parent);
            var decoratedModel = DecorateModel(contentBaseToDecorate, type, config, view);
            var contentDecoratorPresenter = new Decorated_CellContentViewPresenter(decoratedModel, view);

            return decoratedModel;
        }

        private static Decorator_CellContent DecorateModel(CellContentBase contentToDecorate, DecoratorType type,
            IContentDecoratorConfig config, Decorator_CellContentView view)
        {
            int packCount = type.PackCount();
            var previousContent  = GetContentDecorator(contentToDecorate, view, config);
            for (int i = 0; i < packCount - 1; i++)
            {
                previousContent = GetContentDecorator(contentToDecorate: previousContent, view, config);
            }
            return previousContent;
        }

        private static Decorator_CellContent GetContentDecorator(CellContentBase contentToDecorate,
            Decorator_CellContentView view, IContentDecoratorConfig config)
        {
            var decorator = new Decorator_CellContent(contentToDecorate, config.Switchable);
            new Decorated_CellContentViewPresenter(decorator, view);
            return decorator;
        }
    }
}
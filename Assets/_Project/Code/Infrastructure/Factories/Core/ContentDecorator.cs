using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Services;
using UnityEngine;

namespace _Project.Code.Infrastructure.Factories.Core
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

        /// <param name="contentBaseToDecorate"></param>
        /// <param name="type"></param>
        /// <returns>Last decorator</returns>
        public CellContentBase Decorate(CellContentBase contentBaseToDecorate, DecoratorType type)
        {
            if (type == DecoratorType.None)
                return contentBaseToDecorate;

            var config = _decoratorConfigs[type];

            var view = _provider.Instantiate<Decorator_CellContentView>(config.Prefab, _parent);
            var decoratorsModels = DecorateModel(contentBaseToDecorate, type, config);
            var contentDecoratorPresenter = new Decorator_CellContentViewPresenter(decoratorsModels, view);

            return decoratorsModels.Last();
        }

        private static DecoratorCellContent[] DecorateModel(CellContentBase contentToDecorate, DecoratorType type,
            IContentDecoratorConfig config)
        {
            int packCount = type.PackCount();
            DecoratorCellContent[] decorators = new DecoratorCellContent[packCount];

            var nextDecoratedModel = new DecoratorCellContent(contentToDecorate, config.Type, config.Switchable);
            decorators[0] = nextDecoratedModel;
            for (int i = 1; i < packCount; i++)
            {
                nextDecoratedModel = new DecoratorCellContent(nextDecoratedModel, config.Type, config.Switchable);
                decorators[i] = nextDecoratedModel;
            }

            return decorators;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.Particles;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Services;
using UnityEngine;

namespace _Project.Code.Infrastructure.Factories.Core
{
    public class CellContentFactory : ICellContentFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IParticlesPool _particlesPool;
        private readonly Dictionary<DefaultContentType, ICellContentConfig> _cellContentConfigsMap;
        private readonly Transform _parent;
        
        public CellContentFactory(IEnumerable<ICellContentConfig> configs, 
            IAssetProvider assetProvider,
            IParticlesPool particlesPool)
        {
            _assetProvider = assetProvider;
            _particlesPool = particlesPool;
            _cellContentConfigsMap = configs
                .ToDictionary(x => x.DefaultContentType, x => x);

            _parent = new GameObject("CellsContent").transform;
        }

        public DefaultCellContent Create(DefaultContentType type)
        {
            var config = _cellContentConfigsMap[type];

            var view = _assetProvider.Instantiate<CellContentView>(config.Prefab, _parent);
            var model = new DefaultCellContent(config.MatchableContent, config.DefaultContentType, config.Switchable);
            new CellContentViewPresenter(model, view.GetComponent<CellContentView>());

            model.Disabled += ModelOnDisabled;

            return model;
        }

        private void ModelOnDisabled(object sender, EventArgs e)
        {
            var cellContent = (CellContentBase) sender;
            var particlesWrapper = _particlesPool.Get(cellContent.MatchType);
            particlesWrapper.Position = cellContent.Position;
        }
    }
}
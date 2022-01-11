using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Particles;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Code.Infrastructure.Factories
{
    public class CellContentFactory : ICellContentFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IParticlesPool _particlesPool;
        private readonly Dictionary<ContentType, ICellContentConfig> _cellContentConfigsMap;
        private readonly Transform _parent;
        
        public CellContentFactory(IEnumerable<ICellContentConfig> configs, IAssetProvider _assetProvider, IParticlesPool particlesPool)
        {
            this._assetProvider = _assetProvider;
            _particlesPool = particlesPool;
            _cellContentConfigsMap = configs
                .ToDictionary(x => x.ContentType, x => x);

            _parent = new GameObject("CellsContent").transform;
        }

        public CellContent Create(ContentType type)
        {
            var config = _cellContentConfigsMap[type];

            var view = _assetProvider.Instantiate<CellContentView>(config.Prefab, _parent);
            var model = new CellContent(config);
            new CellContentViewPresenter(model, view.GetComponent<CellContentView>());

            model.Disabled += ModelOnDisabled;

            return model;
        }

        private void ModelOnDisabled(object sender, EventArgs e)
        {
            var cellContent = (CellContent) sender;
            var particlesWrapper = _particlesPool.Get(cellContent.Type);
            particlesWrapper.Position = cellContent.Position;
        }
    }
}
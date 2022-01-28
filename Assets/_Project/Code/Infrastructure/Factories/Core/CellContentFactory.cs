using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.Particles;
using _Project.Code.Core.Models.Interfaces;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Loading;
using _Project.Code.Meta.Views.Audio;
using _Project.Code.Meta.Views.Hud;
using UnityEngine;

namespace _Project.Code.Infrastructure.Factories.Core
{
    public class CellContentFactory : ICellContentFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IParticlesPool _particlesPool;
        private readonly AudioPlayer _audioPlayer;
        private readonly Dictionary<DefaultContentType, ICellContentConfig> _cellContentConfigsMap;
        private readonly Transform _parent;
        
        public CellContentFactory(IEnumerable<ICellContentConfig> configs, 
            IAssetProvider assetProvider,
            IParticlesPool particlesPool, 
            AudioPlayer _audioPlayer)
        {
            _assetProvider = assetProvider;
            _particlesPool = particlesPool;
            this._audioPlayer = _audioPlayer;
            _cellContentConfigsMap = configs
                .ToDictionary(x => x.DefaultContentType, x => x);

            _parent = new GameObject("CellsContent").transform;
        }

        public DefaultCellContent Create(DefaultContentType type)
        {
            var config = _cellContentConfigsMap[type];

            var view = _assetProvider.Instantiate<DefaultCellContentView>(config.Prefab, _parent);
            view.GetComponentInChildren<CellContentAudioEffect>().Construct(_audioPlayer);
            var model = new DefaultCellContent(config.MatchableContent, config.DefaultContentType, config.Switchable);
            new CellContentViewPresenter(model, view.GetComponent<DefaultCellContentView>());

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
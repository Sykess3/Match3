using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Installers.Factories;
using _Project.Code.Infrastructure.Services;
using Object = UnityEngine.Object;

namespace _Project.Code.Infrastructure.Factories
{
    public class CellContentFactory : ICellContentFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<Cell.ContentType, ICellContentConfig> _cellContentConfigsMap;
        private readonly List<CellContentViewPresenter> _presenters;

        public CellContentFactory(IAssetProvider assetProvider, IEnumerable<ICellContentConfig> configs)
        {
            _assetProvider = assetProvider;
            _cellContentConfigsMap = configs
                .ToDictionary(x => x.ContentType, x => x);
            _presenters = new List<CellContentViewPresenter>(90);
        }

        public Cell.Content Create(Cell.ContentType type)
        {
            var config = _cellContentConfigsMap[type];
            
            var view = Object.Instantiate(config.Prefab);
            var model = new Cell.Content(config);
            var presenter = new CellContentViewPresenter(model, view.GetComponent<CellContentView>());
            
            CachePresenter(presenter);
            return model;
        }

        private void CachePresenter(CellContentViewPresenter presenter)
        {
            presenter.Destroyed += PresenterOnDestroyed;
            _presenters.Add(presenter);
        }

        private void PresenterOnDestroyed(object sender, EventArgs e)
        {
            var cellContentViewPresenter = (CellContentViewPresenter) sender;
            cellContentViewPresenter.Destroyed -= PresenterOnDestroyed;

            _presenters.Remove(cellContentViewPresenter);
        }
    }
}
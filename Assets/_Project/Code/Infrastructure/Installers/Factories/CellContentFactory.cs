using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using _Project.Code.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Factories
{
    public class CellContentFactory : IFactory<Cell.ContentType, Cell.Content>
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
            switch (type)
            {
                case Cell.ContentType.Red:
                    return InternalCreate(PrefabPath.ContentType.Red, type);
                case Cell.ContentType.Blue:
                    return InternalCreate(PrefabPath.ContentType.Blue, type);
                case Cell.ContentType.Orange:
                    return InternalCreate(PrefabPath.ContentType.Orange, type);
                case Cell.ContentType.Purple:
                    return InternalCreate(PrefabPath.ContentType.Purple, type);
                case Cell.ContentType.Green:
                    return InternalCreate(PrefabPath.ContentType.Green, type);
                case Cell.ContentType.Yellow:
                    return InternalCreate(PrefabPath.ContentType.Yellow, type);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private Cell.Content InternalCreate(string path, Cell.ContentType contentType)
        {
            var view = _assetProvider.Instantiate<CellContentView>(path);
            var model = new Cell.Content(_cellContentConfigsMap[contentType]);
            var presenter = new CellContentViewPresenter(model, view);
            
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
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Presenters;
using _Project.Code.Core.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Code.Infrastructure.Factories
{
    public class CellContentFactory : ICellContentFactory
    {
        private readonly Dictionary<ContentType, ICellContentConfig> _cellContentConfigsMap;
        private readonly Transform parent;
        
        public CellContentFactory(IEnumerable<ICellContentConfig> configs)
        {
            _cellContentConfigsMap = configs
                .ToDictionary(x => x.ContentType, x => x);

            parent = new GameObject("CellsContent").transform;
        }

        public CellContent Create(ContentType type, Vector2 position)
        {
            var config = _cellContentConfigsMap[type];

            var view = Object.Instantiate(config.Prefab, parent);
            var model = new CellContent(config)
            {
                Position = position
            };
            var presenter = new CellContentViewPresenter(model, view.GetComponent<CellContentView>());

            return model;
        }
    }
}
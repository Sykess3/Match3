using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Interfaces;

namespace _Project.Code.Core
{
    public class CellContentObjectPool : ICellContentObjectPool
    {
        private readonly ICellContentFactory _factory;
        private readonly Dictionary<ContentType, Queue<CellContent>> _content;
        
        public CellContentObjectPool(ICellContentFactory factory)
        {
            _factory = factory;
            _content = new Dictionary<ContentType, Queue<CellContent>>();
        }

        public CellContent Get(ContentType type)
        {
            if (!_content.ContainsKey(type)) 
                _content.Add(type, new Queue<CellContent>());
            if (_content[type].Count == 0) 
                AddObjects(type, 1);

            var objectPoolItem = _content[type].Dequeue();
            objectPoolItem.Enable();
            return objectPoolItem;
        }

        private void ReturnToPool(CellContent cellContentToReturn) => 
            _content[cellContentToReturn.Type].Enqueue(cellContentToReturn);

        private void AddObjects(ContentType type, int count)
        {
            for (int i = 0; i < count; i++)
            {
                CellContent cellContent = _factory.Create(type);
                cellContent.Disabled += OnDisabled;
                _content[type].Enqueue(cellContent);
            }
        }

        private void OnDisabled(object sender, EventArgs e)
        {
            var cellContent = (CellContent) sender;
            cellContent.Disabled -= OnDisabled;
            ReturnToPool(cellContent);
        }
    }
}
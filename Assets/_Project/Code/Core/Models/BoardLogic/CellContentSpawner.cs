using System;
using System.Collections.Generic;
using System.IO;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.Interfaces;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class CellContentSpawner : ICellContentSpawner
    {
        private readonly ICellContentPool _cellContentPool;
        private readonly CellCollection _cellCollection;

        public CellContentSpawner(ICellContentPool cellContentPool, CellCollection cellCollection)
        {
            _cellContentPool = cellContentPool;
            _cellCollection = cellCollection;
        }
        

        public void Spawn(ContentToSpawn contentToSpawn)
        {
            var cellContent = _cellContentPool.Get(contentToSpawn.Type);
            cellContent.Position = contentToSpawn.Position;
            
            if (_cellCollection.TryGetCell(contentToSpawn.Position, out var cell))
                cell.Content = cellContent;
            else
                throw new InvalidDataException();
        }

        public void Spawn(IEnumerable<ContentToSpawn> contentToSpawn)
        {
            foreach (var content in contentToSpawn)
                Spawn(content);
        }
    }
}
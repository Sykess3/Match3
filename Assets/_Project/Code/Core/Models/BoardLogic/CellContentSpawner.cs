using System.Collections.Generic;
using System.IO;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class CellContentSpawner : ICellContentSpawner
    {
        private readonly ICellContentFactory _cellContentFactory;
        private readonly CellCollection _cellCollection;

        public CellContentSpawner(ICellContentFactory cellContentFactory, CellCollection cellCollection)
        {
            _cellContentFactory = cellContentFactory;
            _cellCollection = cellCollection;
        }

        public void Spawn(ContentToSpawn contentToSpawn)
        {
            var cellContent = _cellContentFactory.Create(contentToSpawn.Type, contentToSpawn.Position);
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
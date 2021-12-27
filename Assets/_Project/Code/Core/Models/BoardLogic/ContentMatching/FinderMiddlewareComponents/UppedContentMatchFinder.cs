using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents
{
    public class UppedContentMatchFinder 
    {
        private readonly CellCollection _cellCollection;

        public UppedContentMatchFinder(CellCollection cellCollection)
        {
            _cellCollection = cellCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>content to spawn upped content</returns>
        public bool TryFindUppedContentToSpawn(DefaultContentMatchFinder.MatchData matchData, out ContentToSpawn contentToSpawn)
        {
            contentToSpawn = null;
            bool hadCrossMatch = matchData.MatchedInVertical.Count > 0 && matchData.MatchedInHorizontal.Count > 0;

            if (hadCrossMatch)
            {
                var centralCell = matchData.MovedCell;
                contentToSpawn = new ContentToSpawn(centralCell.Content.Type.Up(), centralCell.Position);
            }

            return hadCrossMatch;
        }

        public void OpenExistingUppedContent(List<Cell> matchedCells)
        {
            var uppedContent = FindExistingUppedContent(matchedCells).ToList();
            if (uppedContent.Count <= 0)
                return;
            
            var exposedUppedContent = ExposeUppedContent(uppedContent);
            matchedCells.AddRange(exposedUppedContent);
        }


        private IEnumerable<Cell> ExposeUppedContent(List<Cell> uppedContent)
        {
            foreach (var content in uppedContent)
                return _cellCollection.GetCellsInAllDirections(content);

            throw new ArgumentException();
        }


        private IEnumerable<Cell> FindExistingUppedContent(IReadOnlyList<Cell> cells)
        {
            foreach (Cell cell in cells)
                if (cell.Content.Type.HasFlag(ContentType.UppedContent))
                    yield return cell;
        }
    }
}
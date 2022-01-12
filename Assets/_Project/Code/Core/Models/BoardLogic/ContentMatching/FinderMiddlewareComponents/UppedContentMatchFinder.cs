using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
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
                contentToSpawn = new ContentToSpawn(centralCell.Content.Type.GetUppedContent(), centralCell.Position);
            }

            return hadCrossMatch;
        }

        public void OpenExistingUppedContent(HashSet<Cell> matchedCells)
        {
            var uppedContent = FindExistingUppedContent(matchedCells);
            if (!uppedContent.Any())
                return;
            
            var exposedUppedContent = ExposeUppedContent(uppedContent);
            matchedCells.UnionWith(exposedUppedContent);
        }


        private HashSet<Cell> ExposeUppedContent(IEnumerable<Cell> uppedContent)
        {
            HashSet<Cell> matchedContent = new HashSet<Cell>();
            foreach (var content in uppedContent)
                 matchedContent.UnionWith(_cellCollection.GetCellsInAllDirections(content));

            return matchedContent;
        }


        private IEnumerable<Cell> FindExistingUppedContent(HashSet<Cell> cells)
        {
            foreach (Cell cell in cells)
                if (cell.Content.Type.IsUpped())
                    yield return cell;
        }
    }
}
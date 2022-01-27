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
                contentToSpawn = new ContentToSpawn(centralCell.Content.MatchType.GetUppedContent(), centralCell.Position);
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


        private HashSet<Cell> ExposeUppedContent(IEnumerable<Cell> uppedCells)
        {
            HashSet<Cell> matchedContent = new HashSet<Cell>();
            foreach (var uppedCell in uppedCells)
            {
                if (!uppedCell.Content.IsDecorated)
                {
                    var cellsInAllDirections =
                        _cellCollection.GetCellsInAllDirections(uppedCell, DefaultContentType.Stone)
                            .Where(NotEmptyContent);
                    
                    matchedContent.UnionWith(cellsInAllDirections);
                }
            }

            return matchedContent;

            bool NotEmptyContent(Cell x)
            {
                return x.Content.MatchType != DefaultContentType.Empty;
            }
        }


        private IEnumerable<Cell> FindExistingUppedContent(HashSet<Cell> cells)
        {
            foreach (Cell cell in cells)
                if (cell.Content.MatchType.IsUpped())
                    yield return cell;
        }
    }
}
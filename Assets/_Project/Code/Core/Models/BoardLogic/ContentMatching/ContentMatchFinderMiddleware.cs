using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents;
using _Project.Code.Core.Models.Directions;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public class ContentMatchFinderMiddleware : IContentMatchFinder
    {
        private readonly CellCollection _cells;
        private readonly DefaultContentMatchFinder _defaultContentMatchFinder;
        private readonly UppedContentMatchFinder _uppedContentMatchFinder;

        public ContentMatchFinderMiddleware(CellCollection cells,
            DefaultContentMatchFinder defaultContentMatchFinder,
            UppedContentMatchFinder uppedContentMatchFinder)
        {
            _cells = cells;
            _defaultContentMatchFinder = defaultContentMatchFinder;
            _uppedContentMatchFinder = uppedContentMatchFinder;
        }

        public MatchData FindMatch(Cell cell)
        {
            DefaultContentMatchFinder.MatchData defaultMatchData = _defaultContentMatchFinder.Find(cell);
            if (defaultMatchData.GetAll.Count == 0)
                return new MatchData();

            List<Cell> matchedCells = defaultMatchData.GetAll;
            List<ContentToSpawn> resultContentToSpawn = new List<ContentToSpawn>();
            
            _uppedContentMatchFinder.OpenExistingUppedContent(matchedCells);
            
            if (_uppedContentMatchFinder.TryFindUppedContentToSpawn(defaultMatchData,
                out ContentToSpawn contentToSpawn))
                resultContentToSpawn.Add(contentToSpawn);

            return new MatchData
            {
                MatchedCells = matchedCells,
                ContentToSpawn = resultContentToSpawn
            };
        }

        public MatchData FindMatchesByWholeBoard()
        {
            List<Cell> allMatched = new List<Cell>();
            foreach (var cell in _cells.GetAll())
            {
                if (!allMatched.Contains(cell))
                {
                    MatchData current = FindMatch(cell);

                    if (current.MatchedCells.Count > 0)
                        allMatched.AddRange(current.MatchedCells);
                }
            }

            return new MatchData
            {
                MatchedCells = allMatched.Distinct().ToList(),
                ContentToSpawn = new List<ContentToSpawn>()
            };
        }
    }
}


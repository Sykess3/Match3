﻿using System.Collections.Generic;
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
        private readonly BombMatchFinder _bombMatchFinder;

        public ContentMatchFinderMiddleware(CellCollection cells,
            DefaultContentMatchFinder defaultContentMatchFinder,
            UppedContentMatchFinder uppedContentMatchFinder,
            BombMatchFinder bombMatchFinder)
        {
            _cells = cells;
            _defaultContentMatchFinder = defaultContentMatchFinder;
            _uppedContentMatchFinder = uppedContentMatchFinder;
            _bombMatchFinder = bombMatchFinder;
        }

        public MatchData FindMatch(Cell cell)
        {
            DefaultContentMatchFinder.MatchData defaultMatchData = _defaultContentMatchFinder.Find(cell);
            if (defaultMatchData.GetAll.Count == 0)
                return new MatchData();
            
            HashSet<Cell> matchedCells = defaultMatchData.GetAll;
            HashSet<ContentToSpawn> resultContentToSpawn = new HashSet<ContentToSpawn>();
            
            if (_uppedContentMatchFinder.TryFindUppedContentToSpawn(defaultMatchData,
                out ContentToSpawn contentToSpawn))
                resultContentToSpawn.Add(contentToSpawn);

            _bombMatchFinder.TryBlowUpBombs(matchedCells);
            
            _uppedContentMatchFinder.OpenExistingUppedContent(matchedCells);

            return new MatchData
            {
                MatchedCells = matchedCells,
                ContentToSpawn = resultContentToSpawn
            };
        }

        public MatchData FindMatchesByWholeBoard()
        {
            HashSet<Cell> allMatched = new HashSet<Cell>();
            HashSet<ContentToSpawn> contentToSpawn = new HashSet<ContentToSpawn>();
            foreach (var cell in _cells.GetAll())
            {
                MatchData current = FindMatch(cell);

                if (current.MatchedCells.Count > 0)
                {
                    allMatched.UnionWith(current.MatchedCells);
                    contentToSpawn.UnionWith(current.ContentToSpawn);
                }
                
            }

            return new MatchData
            {
                MatchedCells = allMatched,
                ContentToSpawn = contentToSpawn
            };
        }
    }
}

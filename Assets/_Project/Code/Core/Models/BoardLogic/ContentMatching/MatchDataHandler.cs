using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.Gravity;
using _Project.Code.Core.Models.BoardLogic.Spawner;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching
{
    public class MatchDataHandler
    {
        private readonly BoardGravity _boardGravity;
        private readonly ICellContentSpawner _spawner;
        
        private int _receivedMatchedCells;
        private MatchData _currentMatchData;

        public MatchDataHandler(BoardGravity boardGravity, ICellContentSpawner spawner)
        {
            _boardGravity = boardGravity;
            _spawner = spawner;
        }
        public void Handle(MatchData matchData)
        {
            _currentMatchData = matchData;
            if (!matchData.MatchedCells.Any())
                return;

            MatchContent(matchData.MatchedCells);
            HitDecorators(matchData.Decorators);
        }

        private void MatchContent(HashSet<Cell> cellToMatchContent)
        {
            foreach (var cell in cellToMatchContent)
            {
                cell.Content.Disabled += OnContentDestroy;
                cell.MatchContent();
            }
        }

        private void HitDecorators(HashSet<Cell> decorators)
        {
            foreach (var cell in decorators) 
                cell.MatchContent();
        }

        private void OnContentDestroy(object sender, EventArgs e)
        {
            var cellContent = (CellContentBase) sender;
            cellContent.Disabled -= OnContentDestroy;
            
            _receivedMatchedCells++;

            bool notAllContentDestroyed = _receivedMatchedCells != _currentMatchData.MatchedCells.Count;
            if (notAllContentDestroyed) 
                return;
            
            _spawner.Spawn(_currentMatchData.ContentToSpawn);

            var emptyCells = _currentMatchData
                .CellsToFill
                .ToArray();
            
            _boardGravity.FillContentOnEmptyCells(emptyCells);

            _receivedMatchedCells = 0;
        }
    }
}
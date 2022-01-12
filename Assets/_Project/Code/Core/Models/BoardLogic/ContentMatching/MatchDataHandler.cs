using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;

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
        }

        private void MatchContent(HashSet<Cell> cellToMatchContent)
        {
            foreach (var cell in cellToMatchContent)
            {
                cell.Content.Disabled += OnContentDestroy;
                cell.MatchContent();
            }
        }

        private void OnContentDestroy(object sender, EventArgs e)
        {
            var cellContent = (CellContent) sender;
            cellContent.Disabled -= OnContentDestroy;
            
            _receivedMatchedCells++;

            bool notAllContentDestroyed = _receivedMatchedCells != _currentMatchData.MatchedCells.Count;
            if (notAllContentDestroyed) 
                return;
            
            _spawner.Spawn(_currentMatchData.ContentToSpawn);

            FillContentOnEmptyCells();

            _receivedMatchedCells = 0;
        }

        private void FillContentOnEmptyCells()
        {
            var cellsToFillContent = _currentMatchData.MatchedCellsWithoutDuplicatesInContentToSpawn;
            var sortedEmptyCells = cellsToFillContent.OrderBy(LinqArgs.YPosition).ToArray();
            _boardGravity.FillContentOnEmptyCells(sortedEmptyCells);
        }
    }
}
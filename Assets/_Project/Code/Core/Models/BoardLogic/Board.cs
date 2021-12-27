using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.Swap;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class Board : IModel, IInitializable, IDisposable
    {
        private readonly BoardGravity _boardGravity;
        private readonly SwapCommandHandler _swapCommandHandler;
        private readonly CellCollection _cellCollection;
        private readonly ICellContentSpawner _spawner;
        private readonly IContentMatchFinder _matchFinder;
        public event Action<CellContent> ContentMatched;

        public Board(SwapCommandHandler swapCommandHandler,
            BoardGravity boardGravity,
            CellCollection cellCollection,
            ICellContentSpawner spawner, IContentMatchFinder matchFinder)
        {
            _swapCommandHandler = swapCommandHandler;
            _boardGravity = boardGravity;
            _cellCollection = cellCollection;
            _spawner = spawner;
            _matchFinder = matchFinder;
        }

        public bool TryGetCell(Vector2 position, out Cell cell) => _cellCollection.TryGetCell(position, out cell);

        public IEnumerable<Cell> GetNeighboursOf(Cell cell) => _cellCollection.GetNeighboursOf(cell);

        /// <summary>
        /// Swap cellsContent and match they if condition is satisfied(3 content is the same)
        /// if it not satisfied swap back
        /// </summary>
        public void TryMatch(SwapCommand swapCommand) => _swapCommandHandler.Swap(swapCommand);

        void IInitializable.Initialize()
        {
            _cellCollection.Initialize(OnCellContentStartedMovement);
            _boardGravity.FallingEnded += ResolveMatchesInWholeBoard;
            _swapCommandHandler.Matched += HandleMatchData;
        }

        void IDisposable.Dispose()
        {
            _cellCollection.CleanUp();
            _boardGravity.FallingEnded -= ResolveMatchesInWholeBoard;
            _swapCommandHandler.Matched -= HandleMatchData;
        }

        private void OnCellContentStartedMovement(Cell obj) => _boardGravity.FillContentOnEmptyCell(obj);

        private async void ResolveMatchesInWholeBoard()
        {
            var matchDataByWholeBoard = await Task.Run(_matchFinder.FindMatchesByWholeBoard);
            
            HandleMatchData(matchDataByWholeBoard);
        }

        private void HandleMatchData(MatchData matchData)
        {
            if (!matchData.MatchedCells.Any()) 
                return;
            
            DestroyContent(matchData.MatchedCells);
            _spawner.Spawn(matchData.ContentToSpawn);

            var cellsToFillContent = matchData.MatchedCellsWithoutDuplicatesInContentToSpawn;
            _boardGravity.FillContentOnEmptyCells(cellsToFillContent.ToArray());
            foreach (var matchedCell in matchData.MatchedCells)
                ContentMatched?.Invoke(matchedCell.Content);
        }

        private void DestroyContent(IEnumerable<Cell> cellToDestroyContent)
        {
            foreach (var cell in cellToDestroyContent) 
                cell.Content.Destroy();
        }
    }
}
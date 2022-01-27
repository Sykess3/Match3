using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.Gravity;
using _Project.Code.Core.Models.BoardLogic.Swap;
using _Project.Code.Core.Models.Framework;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class Board : IModel, IInitializable, IDisposable
    {
        private readonly BoardGravity _boardGravity;
        private readonly SwapCommandHandler _swapCommandHandler;
        private readonly CellCollection _cellCollection;
        private readonly MatchDataHandler _matchDataHandler;
        private readonly IContentMatchFinder _matchFinder;
        public event Action<CellContentBase> DefaultMatched;
        public event Action<CellContentBase> DecoratedMatched;

        public event Action FallingEnded;

        public Board(SwapCommandHandler swapCommandHandler,
            BoardGravity boardGravity,
            CellCollection cellCollection,
            MatchDataHandler matchDataHandler,
            IContentMatchFinder matchFinder)
        {
            _swapCommandHandler = swapCommandHandler;
            _boardGravity = boardGravity;
            _cellCollection = cellCollection;
            _matchDataHandler = matchDataHandler;
            _matchFinder = matchFinder;
        }

        public bool TryGetCell(Vector2 position, out Cell cell) => _cellCollection.TryGetCell(position, out cell);

        public IEnumerable<Cell> GetNeighboursOf(Cell cell) => _cellCollection.GetNeighboursOf(cell);

        /// <summary>
        /// Swap cellsContent and match they if condition is satisfied(3 content is the same)
        /// if it not satisfied swap back
        /// </summary>
        public void TryMatch(SwapCommand swapCommand, Action onSwapSucceed) => _swapCommandHandler.Swap(swapCommand, onSwapSucceed);

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
            foreach (var cell in matchData.MatchedCells) 
                DefaultMatched?.Invoke(cell.Content);
            foreach (var cell in matchData.Decorators) 
                DecoratedMatched?.Invoke(cell.Content);

            _matchDataHandler.Handle(matchData);
            
            bool isNoCellsToFill = matchData.CellsToFill.Count == 0;
            if (isNoCellsToFill) 
                FallingEnded?.Invoke();
        }
        
    }
}
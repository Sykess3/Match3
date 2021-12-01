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
        private readonly ContentMatcher _matcher;
        private readonly SwapCommandHandler _swapCommandHandler;
        private readonly CellCollection _cellCollection;

        public event Action<CellContent> ContentMatched;

        public Board(ContentMatcher matcher,
            SwapCommandHandler swapCommandHandler,
            BoardGravity boardGravity,
            CellCollection cellCollection)
        {
            _matcher = matcher;
            _swapCommandHandler = swapCommandHandler;
            _boardGravity = boardGravity;
            _cellCollection = cellCollection;
        }

        void IInitializable.Initialize()
        {
            _cellCollection.Initialize(OnCellContentStartedMovement);
            _boardGravity.FallingEnded += ResolveMatchesInWholeBoard;
            _swapCommandHandler.Matched += OnMatched;
        }
        void IDisposable.Dispose()
        {
            _cellCollection.CleanUp();
           _boardGravity.FallingEnded -= ResolveMatchesInWholeBoard;
            _swapCommandHandler.Matched -= OnMatched;
        }

        public bool TryGetCell(Vector2 position, out Cell cell) => _cellCollection.TryGetCell(position, out cell);

        public IEnumerable<Cell> GetNeighboursOf(Cell cell) => _cellCollection.GetNeighboursOf(cell);
        
        /// <summary>
        /// Swap cellsContent and match they if condition is satisfied(3 content is the same)
        /// if it not satisfied swap back
        /// </summary>
        public void TryMatch(SwapCommand swapCommand) => _swapCommandHandler.Swap(swapCommand);

        private void ResolveMatchesInWholeBoard()
        {
            _matcher.ResolveMatchesByWholeBoard(OnMatched);
        }

        private void OnCellContentStartedMovement(Cell obj)
        {
            _boardGravity.FillContentOnEmptyCell(obj);
        }
        
        private void OnMatched(IEnumerable<Cell> matchedCells)
        {
            if (matchedCells.Any())
            {
                _boardGravity.FillContentOnEmptyCells(matchedCells.ToArray());
            
                foreach (var matchedCell in matchedCells) 
                    ContentMatched?.Invoke(matchedCell.Content);
            }
        }
        
    }
}
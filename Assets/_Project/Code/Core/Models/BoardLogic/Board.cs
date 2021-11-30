using System;
using System.Collections.Generic;
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
        private readonly CellContentFalling _contentFalling;
        private readonly IContentMatchFinder _matchFinder;
        private readonly SwapCommandHandler _swapCommandHandler;
        private readonly CellCollection _cellCollection;

        public event Action<CellContent> ContentMatched;

        public Board(IContentMatchFinder matchFinder,
            SwapCommandHandler swapCommandHandler,
            CellContentFalling contentFalling,
            CellCollection cellCollection)
        {
            _matchFinder = matchFinder;
            _swapCommandHandler = swapCommandHandler;
            _contentFalling = contentFalling;
            _cellCollection = cellCollection;
        }

        void IInitializable.Initialize()
        {
            _cellCollection.Initialize(OnCellContentMatched, OnCellContentStartedMovement);
            _contentFalling.FallingEnded += FindAnyMatchesInWholeBoard;
        }

        void IDisposable.Dispose()
        {
            _cellCollection.CleanUp();
            _contentFalling.FallingEnded -= FindAnyMatchesInWholeBoard;
        }

        public bool TryGetCell(Vector2 position, out Cell cell) => _cellCollection.TryGetCell(position, out cell);

        public IEnumerable<Cell> GetNeighboursOf(Cell cell) => _cellCollection.GetNeighboursOf(cell);
        
        /// <summary>
        /// Swap cellsContent and match they if condition is satisfied(3 content is the same)
        /// if it not satisfied swap back
        /// </summary>
        public void TryMatch(SwapCommand swapCommand) => _swapCommandHandler.Swap(swapCommand);

        private async void FindAnyMatchesInWholeBoard()
        {
            List<Cell> matchedCells = await Task.Run(_matchFinder.FindMatchesByWholeBoard);
            foreach (var cell in matchedCells) 
                cell.Content.Match();
        }

        private void OnCellContentStartedMovement(Cell obj)
        {
            _contentFalling.FillContentOnEmptyCells(obj);
        }

        private void OnCellContentMatched(object sender, EventArgs eventArgs)
        {
            var cell = (Cell) sender;
            _contentFalling.FillContentOnEmptyCells(cell);
            ContentMatched?.Invoke(cell.Content);
        }
    }
}
using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Swap;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class Board : IModel, IInitializable, IDisposable
    {
        private readonly CellCollectionFactory _cellCollectionFactory;
        private readonly SwapCommandHandlerFactory _swapCommandHandlerFactory;
        private readonly CellContentFallingFactory _contentFallingFactory;

        private CellContentFalling _cellContentFalling;
        private SwapCommandHandler _swapCommandHandler;
        private CellCollection _cells;

        public event Action<CellContent> ContentMatched;

        public Board(
            SwapCommandHandlerFactory swapCommandHandlerFactory,
            CellContentFallingFactory contentFallingFactory,
            CellCollectionFactory cellCollectionFactory)
        {
            _swapCommandHandlerFactory = swapCommandHandlerFactory;
            _contentFallingFactory = contentFallingFactory;
            _cellCollectionFactory = cellCollectionFactory;
        }

        void IInitializable.Initialize()
        {
            _cells = _cellCollectionFactory.CreateWithFilling();

            _swapCommandHandler = _swapCommandHandlerFactory.Create(_cells);
            _cellContentFalling = _contentFallingFactory.Create(_cells);
            
            _cells.Initialize(OnCellContentMatched, OnCellContentStartedMovement);
        }

        void IDisposable.Dispose()
        {
            _cells.CleanUp();
        }

        public bool TryGetCell(Vector2 position, out Cell cell) => _cells.TryGetCell(position, out cell);

        public IEnumerable<Cell> GetNeighboursOf(Cell cell) => _cells.GetNeighboursOf(cell);


        /// <summary>
        /// Swap cellsContent and match they if condition is satisfied(3 content is the same)
        /// if it not satisfied swap back
        /// </summary>
        public void TryMatch(SwapCommand swapCommand)
        {
            _swapCommandHandler.Swap(swapCommand);
        }

        private void OnCellContentStartedMovement(Cell obj)
        {
            _cellContentFalling.FillContentOnEmptyCells(obj);
        }

        private void OnCellContentMatched(object sender, EventArgs eventArgs)
        {
            var cell = (Cell) sender;
            _cellContentFalling.FillContentOnEmptyCells(cell);
            ContentMatched?.Invoke(cell.Content);
        }
    }
}
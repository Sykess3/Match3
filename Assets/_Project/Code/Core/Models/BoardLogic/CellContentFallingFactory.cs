using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Random;

namespace _Project.Code.Core.Models.BoardLogic
{
    public class CellContentFallingFactory
    {
        private readonly ICellContentMover _mover;
        private readonly IPlayerInput _playerInput;
        private readonly IRandomCellContentGenerator _cellRandomized;
        private readonly ICoroutineRunner _coroutineRunner;

        public CellContentFallingFactory(
            ICoroutineRunner coroutineRunner, 
            IRandomCellContentGenerator cellRandomized,
            ICellContentMover mover,
            IPlayerInput _playerInput)
        {
            _coroutineRunner = coroutineRunner;
            _cellRandomized = cellRandomized;
            _mover = mover;
            this._playerInput = _playerInput;
        }

        public CellContentFalling Create(CellCollection cellCollection) =>
            new CellContentFalling(_mover, _cellRandomized, _coroutineRunner, cellCollection, _playerInput);
    }
}
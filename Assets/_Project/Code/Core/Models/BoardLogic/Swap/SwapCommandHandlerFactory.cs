using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;

namespace _Project.Code.Core.Models.BoardLogic.Swap
{
    public class SwapCommandHandlerFactory
    {
        private readonly IContentMatcher _matcher;
        private readonly ICellContentSwapper _contentSwapper;
        private readonly IPlayerInput _playerInput;

        public SwapCommandHandlerFactory(
            IContentMatcher matcher, 
            ICellContentSwapper contentSwapper,
            IPlayerInput playerInput)
        {
            _matcher = matcher;
            _contentSwapper = contentSwapper;
            _playerInput = playerInput;
        }

        public SwapCommandHandler Create(CellCollection cellCollection)
        {
            _matcher.Initialize(cellCollection);
            return new SwapCommandHandler(_matcher, _contentSwapper, _playerInput);
        }
    }
}
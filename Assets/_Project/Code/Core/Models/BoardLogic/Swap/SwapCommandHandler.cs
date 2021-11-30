using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;

namespace _Project.Code.Core.Models.BoardLogic.Swap
{
    public class SwapCommandHandler
    {
        private readonly ContentMatcher _matcher;
        private readonly ICellContentSwapper _cellContentSwapper;
        private readonly IPlayerInput _playerInput;

        public SwapCommandHandler(ContentMatcher matcher, ICellContentSwapper cellContentSwapper, IPlayerInput playerInput)
        {
            _matcher = matcher;
            _cellContentSwapper = cellContentSwapper;
            _playerInput = playerInput;
        }

        public void Swap(SwapCommand command)
        {
            _playerInput.Disable();
            command.Swapper = _cellContentSwapper;
            command.Execute(OnCommandExecuted);
        }

        private void OnCommandExecuted(SwapCommand command)
        {
            if (_matcher.TryMatch(command))
                _playerInput.Enable();
            else
                command.Revert(OnCommandReverted);
        }

        private void OnCommandReverted(SwapCommand obj) => _playerInput.Enable();
    }
}
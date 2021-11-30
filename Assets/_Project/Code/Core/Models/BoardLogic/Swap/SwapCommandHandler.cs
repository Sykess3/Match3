using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;

namespace _Project.Code.Core.Models.BoardLogic.Swap
{
    public class SwapCommandHandler
    {
        private readonly IContentMatcher _matcher;
        private readonly ICellContentSwapper _cellContentSwapper;
        private readonly IPlayerInput _playerInput;

        public SwapCommandHandler(IContentMatcher matcher, ICellContentSwapper cellContentSwapper, IPlayerInput playerInput)
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
            var matchedCells = GetMatchedCells(command);

            if (matchedCells.Any())
            {
                DestroyCells(matchedCells);
                _playerInput.Enable();
            }
            else
            {
                command.Revert(OnCommandReverted);
            }
        }

        private void OnCommandReverted(SwapCommand obj) => _playerInput.Enable();


        private List<Cell> GetMatchedCells(SwapCommand command)
        {
            var matchedCells1 = _matcher.FindMatch(command.FirstCell);

            var matchedCells = matchedCells1
                .Union(_matcher.FindMatch(command.SecondCell))
                .ToList();
            return matchedCells;
        }


        private void DestroyCells(IEnumerable<Cell> matchedCells)
        {
            foreach (var cell in matchedCells) 
                cell.Content.Match();
        }
    }
}
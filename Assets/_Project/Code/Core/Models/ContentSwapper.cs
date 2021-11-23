using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Infrastructure;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Models
{
    public class ContentSwapper
    {
        private readonly IContentMatcher _matcher;
        private readonly IPlayerInput _playerInput;

        public ContentSwapper(IContentMatcher matcher, IPlayerInput playerInput)
        {
            _matcher = matcher;
            _playerInput = playerInput;
        }

        public void Switch(SwapCommand command)
        {
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

        private void OnCommandReverted(SwapCommand command)
        {
            _playerInput.Enable();
        }

        private List<Cell> GetMatchedCells(SwapCommand command)
        {
            var matchedCells1 = _matcher.Match(command.FirstCell);

            var matchedCells = matchedCells1
                .Union(_matcher.Match(command.SecondCell))
                .ToList();
            return matchedCells;
        }


        private void DestroyCells(IEnumerable<Cell> matchedCells)
        {
            foreach (var cell in matchedCells)
            {
                cell.Filler.Destroy();
            }
        }
    }
}
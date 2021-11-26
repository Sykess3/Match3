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
    public class SwapCommandHandler
    {
        private readonly IContentMatcher _matcher;

        public SwapCommandHandler(IContentMatcher matcher)
        {
            _matcher = matcher;
        }

        public void Swap(SwapCommand command)
        {
            command.Execute(OnCommandExecuted);
        }

        private void OnCommandExecuted(SwapCommand command)
        {
            var matchedCells = GetMatchedCells(command);

            if (matchedCells.Any())
                DestroyCells(matchedCells);
            else
                command.Revert();
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
                cell.Content.Match();
        }
    }
}
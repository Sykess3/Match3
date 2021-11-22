using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace _Project.Code.Core.Models
{
    public class ContentSwitcher
    {
        private readonly IContentMatcher _matcher;

        public ContentSwitcher(IContentMatcher matcher)
        {
            _matcher = matcher;
        }

        public void Switch(SwitchCommand command)
        {
            command.Execute();
            var matchedCells1 = _matcher.Match(command.Cell1);
            var matchedCells2 = _matcher.Match(command.Cell2);

            if (!matchedCells1.Any() && !matchedCells2.Any())
            {
                command.Revert();
                return;
            }

            DestroyMatchedCells(matchedCells1, matchedCells2);
        }

        private static void DestroyMatchedCells(IReadOnlyCollection<Cell> matchedCells1, IReadOnlyCollection<Cell> matchedCells2)
        {
            var uniqueMatchedCells = matchedCells1.Union(matchedCells2);
            foreach (var cell in uniqueMatchedCells)
                cell.Filler.Destroy();
        }
    }
}
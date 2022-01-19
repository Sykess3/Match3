using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using ModestTree;

namespace _Project.Code.Core.Models.BoardLogic.ContentMatching.FinderMiddlewareComponents
{
    public class BombMatchFinder
    {
        private readonly CellCollection _cellCollection;
        private readonly Dictionary<ContentType, IBombConfig> _bombConfigs;
        private readonly System.Random _systemRandom = new System.Random();

        public BombMatchFinder(CellCollection cellCollection, IEnumerable<IBombConfig> configs)
        {
            _cellCollection = cellCollection;
            _bombConfigs = configs.ToDictionary(x => x.ContentType, x => x);
        }

        public void TryBlowUpBombs(HashSet<Cell> matchedCells)
        {
            var bombs = FindBombs(matchedCells);
            if (bombs == null)
                return;

            HashSet<Cell> alreadyBlowedUp = new HashSet<Cell>();
            foreach (Cell bomb in bombs)
            {
                var blowedUp = BlowUp(bomb, alreadyDestroyedContent: alreadyBlowedUp);
                alreadyBlowedUp.UnionWith(blowedUp);
            }

            matchedCells.UnionWith(alreadyBlowedUp);
        }

        private IEnumerable<Cell> BlowUp(Cell cell, IEnumerable<Cell> alreadyDestroyedContent)
        {
            var bombType = cell.Content.MatchType.GetBombType();
            var defaultCellsOfBombType = _cellCollection
                .GetAll(bombType)
                .Except(alreadyDestroyedContent);

            var uppedCellsOfBombType = _cellCollection
                .GetAll(bombType.GetUppedContent())
                .Except(alreadyDestroyedContent);

            HashSet<Cell> cellsOfBombType = new HashSet<Cell>(defaultCellsOfBombType);
            cellsOfBombType.UnionWith(uppedCellsOfBombType);

            int amountCellToDestroy = GetAmountCellToDestroy(cell.Content.MatchType);
            return BlowUpRandomCells(among: cellsOfBombType, amount: amountCellToDestroy);
        }

        private IEnumerable<Cell> BlowUpRandomCells(HashSet<Cell> among, int amount)
        {
            if (amount >= among.Count)
                return among;

            int blowedUpCellsAlready = 0;
            Cell[] blowedUpCells = new Cell[amount];
            while (true)
            {
                foreach (var cell in among)
                {
                    if (blowedUpCellsAlready >= amount)
                        return blowedUpCells;

                    if (_systemRandom.RandomBoolean() && !blowedUpCells.Contains(cell))
                    {
                        blowedUpCells[blowedUpCellsAlready] = cell;
                        blowedUpCellsAlready++;
                    }
                }
            } 
        }

        private int GetAmountCellToDestroy(ContentType bombType)
        {
            int max = _bombConfigs[bombType].MaxContentAmountToDestroy;
            int min = _bombConfigs[bombType].MinContentAmountToDestroy;
            return UnityEngine.Random.Range(min, max + 1);
        }

        private IEnumerable<Cell> FindBombs(HashSet<Cell> matchedCells)
        {
            foreach (var cell in matchedCells)
            {
                if (cell.Content.MatchType.IsBomb())
                    yield return cell;
            }
        }
    }
}
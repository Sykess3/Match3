using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells;
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

        public void TryBlowUpBombs(List<Cell> matchedCells)
        {
            var bombs = FindBombs(matchedCells);
            if (bombs == null)
                return;

            List<Cell> allBlowedUp = new List<Cell>();
            foreach (Cell bomb in bombs)
            {
                var blowedUp = BlowUp(bomb, alreadyDestroyedContent: allBlowedUp);
                allBlowedUp.AddRange(blowedUp);
            }

            var result = allBlowedUp.Distinct();
            matchedCells.AddRange(result);
        }

        private IEnumerable<Cell> BlowUp(Cell cell, List<Cell> alreadyDestroyedContent)
        {
            var bombType = cell.Content.Type.GetBombType();
            var defaultCellsOfBombType = _cellCollection
                .GetAll(bombType)
                .Except(alreadyDestroyedContent);

            var uppedCellsOfBombType = _cellCollection
                .GetAll(bombType.GetUppedContent())
                .Except(alreadyDestroyedContent);

            var cellsOfBombType = defaultCellsOfBombType
                .Concat(uppedCellsOfBombType)
                .ToList();

            int amountCellToDestroy = GetAmountCellToDestroy(cell.Content.Type);
            return BlowUpRandomCells(among: cellsOfBombType, amount: amountCellToDestroy);
        }

        private List<Cell> BlowUpRandomCells(List<Cell> among, int amount)
        {
            if (amount >= among.Count)
                return among;

            int blowedUpCellsAlready = 0;
            List<Cell> blowedUpCells = new List<Cell>(amount);
            while (true)
            {
                for (int i = 0; i < among.Count; i++)
                {
                    if (blowedUpCellsAlready >= amount)
                        return blowedUpCells;

                    if (_systemRandom.RandomBoolean() && !blowedUpCells.Contains(among[i]))
                    {
                        blowedUpCells.Add(among[i]);
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

        private IEnumerable<Cell> FindBombs(List<Cell> matchedCells)
        {
            for (int i = 0; i < matchedCells.Count; i++)
            {
                if (matchedCells[i].Content.Type.IsBomb())
                    yield return matchedCells[i];
            }
        }
    }
}
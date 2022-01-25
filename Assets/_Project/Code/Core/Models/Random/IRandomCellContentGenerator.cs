using System;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEngine;

namespace _Project.Code.Core.Models.Random
{
    public interface IRandomCellContentGenerator
    {
        CellContentBase Generate(Vector2 position);
        CellContentBase GenerateUnmatchable(Vector2 position, Tuple<Cell, Cell> southNeighbours,
            Tuple<Cell, Cell> westNeighbours);
    }
}
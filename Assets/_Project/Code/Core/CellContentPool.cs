using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.DataStructures;
using _Project.Code.Core.Models.Interfaces;
using UnityEngine;

namespace _Project.Code.Core
{
    public class CellContentPool : ObjectPool<ContentType, CellContent>, ICellContentPool
    {
        public CellContentPool(ICellContentFactory factory) : base(factory)
        {
        }
    }
}
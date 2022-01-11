using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.DataStructures;
using _Project.Code.Core.Models.Interfaces;
using UnityEngine;

namespace _Project.Code.Core
{
    public class CellContentObjectPool : ObjectPool<ContentType, CellContent>, ICellContentObjectPool
    {
        public CellContentObjectPool(ICellContentFactory factory) : base(factory)
        {
        }
    }
}
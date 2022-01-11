using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.DataStructures;
using UnityEngine;

namespace _Project.Code.Core.Models.Interfaces
{
    public interface ICellContentFactory : IObjectPoolFactory<ContentType, CellContent>
    {
        
    }
}
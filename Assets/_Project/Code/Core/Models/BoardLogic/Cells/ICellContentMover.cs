using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    public interface ICellContentMover
    {
        void MoveCellContent(Cell from, Cell to, Action callback = null);
        void MoveCellContent(CellContent contentToMove, Cell to, Action callback = null);

        void MoveCellContent(Cell @from, Cell to, ContentRoute route, Action callback = null);
        
        void MoveCellContent(CellContent contentToMove, Cell to, ContentRoute route, Action callback = null);
    }
    
    public class ContentRoute 
    {
        private readonly Stack<Vector2> _points;

        public int Count => _points.Count;

        public ContentRoute(Vector2 targetPosition)
        {
            _points = new Stack<Vector2>();
            _points.Push(targetPosition);
        }

        public void AddPoint(Vector2 point)
        {
            _points.Push(point);
        }

        public Vector2 PopPoint() => _points.Pop();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Code.Core.Input;
using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic.Gravity
{
    public class BoardGravity
    {
        private readonly ICellContentFalling _contentFalling;
        private readonly IPlayerInput _playerInput;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly SortedList<Cell> _cellsToFill;

        public Action FallingEnded;

        public BoardGravity(
            ICellContentFalling contentFalling,
            IPlayerInput playerInput, 
            ICoroutineRunner coroutineRunner,
            CellCollection cellCollection)
        {
            _contentFalling = contentFalling;
            _playerInput = playerInput;
            _coroutineRunner = coroutineRunner;

            _cellsToFill = new SortedList<Cell>(new ByYCellPosition(cellCollection));
        }


        public void FillContentOnEmptyCell(Cell emptyCell)
        {
            if (_cellsToFill.Count == 0)
                _coroutineRunner.StartCoroutine(StartFilling());

            AddToCellsToFill(emptyCell);
        }

        public void FillContentOnEmptyCells(Cell[] emptyCells)
        {
            if (_cellsToFill.Count == 0)
                _coroutineRunner.StartCoroutine(StartFilling());

            AddToCellsToFill(emptyCells);
        }

        private void AddToCellsToFill(Cell emptyCell)
        {
            _cellsToFill.Add(emptyCell);
        }

        private void AddToCellsToFill(Cell[] emptyCells)
        {
            for (int i = 0; i < emptyCells.Length; i++)
            {
                _cellsToFill.Add(emptyCells[i]);
            }
        }

        private IEnumerator StartFilling()
        {
            yield return null;

            _playerInput.Disable();
            for (int i = 0; i < _cellsToFill.Count; i++)
            {
                if (!_contentFalling.TryFillContentOnEmptyCell(_cellsToFill[i], OnCellLanded))
                {
                    _cellsToFill.Remove(_cellsToFill[i]);
                    i--;
                }
            }

            if (_cellsToFill.Count == 0) 
                OnFallingEnded();
        }

        private void OnCellLanded(Cell cell)
        {
            _cellsToFill.Remove(cell);
            if (_cellsToFill.Count > 0)
                return;

            OnFallingEnded();
        }

        private void OnFallingEnded()
        {
            _playerInput.Enable();
            FallingEnded?.Invoke();
        }
    }

    public class SortedList<T> : ICollection<T>
    {
        private List<T> m_innerList;
        private Comparer<T> m_comparer;

        public SortedList() : this(Comparer<T>.Default)
        {
        }

        public SortedList(Comparer<T> comparer)
        {
            m_innerList = new List<T>();
            m_comparer = comparer;
        }

        public void Add(T item)
        {
            int insertIndex = FindIndexForSortedInsert(m_innerList, m_comparer, item);
            m_innerList.Insert(insertIndex, item);
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire SortedList<T>
        /// </summary>
        public int IndexOf(T item)
        {
            int insertIndex = FindIndexForSortedInsert(m_innerList, m_comparer, item);
            if (insertIndex == m_innerList.Count)
            {
                return -1;
            }

            if (m_comparer.Compare(item, m_innerList[insertIndex]) == 0)
            {
                int index = insertIndex;
                while (index > 0 && m_comparer.Compare(item, m_innerList[index - 1]) == 0)
                {
                    index--;
                }

                return index;
            }

            return -1;
        }

        public bool Remove(T item)
        {
            // int index = IndexOf(item);
            // if (index >= 0)
            // {
            //     m_innerList.RemoveAt(index);
            //     return true;
            // }

            m_innerList.Remove(item);
            return true;
        }

        public void RemoveAt(int index)
        {
            m_innerList.RemoveAt(index);
        }

        public void CopyTo(T[] array)
        {
            m_innerList.CopyTo(array);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_innerList.CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            m_innerList.Clear();
        }

        public T this[int index]
        {
            get { return m_innerList[index]; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_innerList.GetEnumerator();
        }

        public int Count
        {
            get { return m_innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public static int FindIndexForSortedInsert(List<T> list, Comparer<T> comparer, T item)
        {
            if (list.Count == 0)
            {
                return 0;
            }

            int lowerIndex = 0;
            int upperIndex = list.Count - 1;
            int comparisonResult;
            while (lowerIndex < upperIndex)
            {
                int middleIndex = (lowerIndex + upperIndex) / 2;
                T middle = list[middleIndex];
                comparisonResult = comparer.Compare(middle, item);
                if (comparisonResult == 0)
                {
                    return middleIndex;
                }
                else if (comparisonResult > 0) // middle > item
                {
                    upperIndex = middleIndex - 1;
                }
                else // middle < item
                {
                    lowerIndex = middleIndex + 1;
                }
            }

            // At this point any entry following 'middle' is greater than 'item',
            // and any entry preceding 'middle' is lesser than 'item'.
            // So we either put 'item' before or after 'middle'.
            comparisonResult = comparer.Compare(list[lowerIndex], item);
            if (comparisonResult < 0) // middle < item
            {
                return lowerIndex + 1;
            }
            else
            {
                return lowerIndex;
            }
        }
    }

    public class ByYCellPosition : Comparer<Cell>
    {
        private readonly CellCollection _cellCollection;

        public ByYCellPosition(CellCollection cellCollection)
        {
            _cellCollection = cellCollection;
        }

        public override int Compare(Cell x, Cell y)
        {
            if (x.Position.y > y.Position.y)
                return 1;

            if (x.Position.y == y.Position.y)
            {
                bool isStoneAboveX = _cellCollection.IsStoneAbove(x.Position);
                bool isStoneAboveY = _cellCollection.IsStoneAbove(y.Position);
                if (isStoneAboveX && isStoneAboveY)
                    return 0;

                if (isStoneAboveX)
                    return 1;
                
                return -1;
            }

            if (x.Position.y < y.Position.y)
                return -1;

            return 0;
        }
    }
}
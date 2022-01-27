using System;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Directions;
using _Project.Code.Core.Models.Random;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Gravity
{
    // Bullshit refactors this
    public class CellContentFalling : ICellContentFalling
    {
        private const int YMinSpawnPosition = 5;

        private readonly CellCollection _cellCollection;
        private readonly IRandomCellContentGenerator _contentGenerator;
        private readonly ICellContentMover _mover;
        private readonly int[] _ySpawnPositions = new int[Constant.Board.Size.x];
        private bool _isResetSpawnPositions;

        public CellContentFalling(CellCollection cellCollection, IRandomCellContentGenerator contentGenerator,
            ICellContentMover mover)
        {
            _cellCollection = cellCollection;
            _contentGenerator = contentGenerator;
            _mover = mover;

            ResetSpawnPositionsArray();
        }

        public bool TryFillContentOnEmptyCell(Cell emptyCell, Action<Cell> onLandedCallback)
        {
            _isResetSpawnPositions = false;

            var aboveContentType = GetFilledCellAbove(emptyCell.Position, out Cell filledAboveCell,
                out Vector2 beforeImmovablePos);

            if (aboveContentType == FindingContentState.FoundMovable)
            {
                _mover.MoveCellContent(
                    @from: filledAboveCell,
                    to: emptyCell,
                    callback: CallOnLandedCallback);

                return true;
            }

            if (aboveContentType == FindingContentState.GenerateNew)
            {
                GenerateNew(emptyCell, onLandedCallback);
                return true;
            }

            if (aboveContentType == FindingContentState.FoundImmovable)
            {
                ContentRoute route = new ContentRoute(targetPosition: emptyCell.Position);

                var contentStateInDiagonalFinding = GetFilledCellInDiagonal(emptyCell.Position,
                    out Cell filledDiagonalCell, out route, route);

                if (contentStateInDiagonalFinding == FindingContentState.DoesNotExist)
                    return false;

                if (contentStateInDiagonalFinding == FindingContentState.FoundMovable)
                {
                    _mover.MoveCellContent(
                        @from: filledDiagonalCell,
                        to: emptyCell,
                        callback: CallOnLandedCallback);

                    return true;
                }

                if (contentStateInDiagonalFinding == FindingContentState.GenerateNew)
                {
                    GenerateNew(
                        to: emptyCell,
                        route: route,
                        onLandedCallback: onLandedCallback);

                    return true;
                }
            }

            throw new InvalidOperationException();

            void CallOnLandedCallback()
            {
                onLandedCallback?.Invoke(emptyCell);
            }
        }


        private void GenerateNew(Cell to, Action<Cell> onLandedCallback)
        {
            int xIndexOnMatrixOfEmptyCell = Mathf.RoundToInt(to.Position.x + Constant.Board.OffsetFromCenter.x);
            var randomContent = _contentGenerator.Generate(
                new Vector2(to.Position.x, _ySpawnPositions[xIndexOnMatrixOfEmptyCell]));

            _mover.MoveCellContent(
                contentBaseToMove: randomContent,
                to: to,
                callback: OnLandedGeneratedContent);


            _ySpawnPositions[xIndexOnMatrixOfEmptyCell]++;

            void OnLandedGeneratedContent()
            {
                if (!_isResetSpawnPositions)
                {
                    _isResetSpawnPositions = true;
                    ResetSpawnPositionsArray();
                }

                onLandedCallback?.Invoke(to);
            }
        }

        private void GenerateNew(Cell to, ContentRoute route, Action<Cell> onLandedCallback)
        {
            int xIndexOnMatrixOfEmptyCell = Mathf.RoundToInt(route.StartPoint().x + Constant.Board.OffsetFromCenter.x);
            var randomContent = _contentGenerator.Generate(
                new Vector2(route.StartPoint().x, _ySpawnPositions[xIndexOnMatrixOfEmptyCell]));

            _mover.MoveCellContent(
                contentBaseToMove: randomContent,
                to: to,
                route,
                callback: OnLandedGeneratedContent);


            _ySpawnPositions[xIndexOnMatrixOfEmptyCell]++;

            void OnLandedGeneratedContent()
            {
                if (!_isResetSpawnPositions)
                {
                    _isResetSpawnPositions = true;
                    ResetSpawnPositionsArray();
                }

                onLandedCallback?.Invoke(to);
            }
        }


        private FindingContentState GetFilledCellInDiagonal(Vector2 emptyCellPosition, out Cell resultCell,
            out ContentRoute route, ContentRoute beginOfRoute = null)
        {
            bool continueFindInWest = true;
            Vector2 westPosition = emptyCellPosition;
            
            bool continueFindInEast = true;
            Vector2 eastPosition = emptyCellPosition;
            
            ContentRoute westRoute;
            ContentRoute eastRoute;
            if (beginOfRoute != null)
            {
                westRoute = new ContentRoute(targetPosition: emptyCellPosition, beginOfRoute);
                eastRoute = new ContentRoute(targetPosition: emptyCellPosition, beginOfRoute);
            }
            else
            {
                westRoute = new ContentRoute(targetPosition: emptyCellPosition);
                eastRoute = new ContentRoute(targetPosition: emptyCellPosition);
            }

            while (continueFindInWest || continueFindInEast)
            {
                if (continueFindInEast)
                {
                    FindingContentState eastContentFindingState =
                        GetCellFromDiagonallyUpwardNeighbourOrNeighboursAboveCell(eastPosition, Direction.East,
                            out resultCell, out Vector2 beforeImmovablePos);

                    eastPosition += new Vector2(1, 1);
                    eastRoute.AddPoint(eastPosition);

                    if (eastContentFindingState == FindingContentState.FoundMovable ||
                        eastContentFindingState == FindingContentState.GenerateNew)
                    {
                        route = eastRoute;
                        return eastContentFindingState;
                    }

                    if (eastContentFindingState == FindingContentState.ImmovableDiagonalNeighbour ||
                        eastContentFindingState == FindingContentState.BeyondTheBoard)
                    {
                        continueFindInEast = false;
                    }
                }

                if (continueFindInWest)
                {
                    FindingContentState westContentFindingState =
                        GetCellFromDiagonallyUpwardNeighbourOrNeighboursAboveCell(westPosition, Direction.West,
                            out resultCell, out Vector2 beforeImmovablePos);

                    westPosition += new Vector2(-1, 1);
                    westRoute.AddPoint(westPosition);

                    if (westContentFindingState == FindingContentState.FoundMovable ||
                        westContentFindingState == FindingContentState.GenerateNew)
                    {
                        route = westRoute;
                        return westContentFindingState;
                    }

                    if (westContentFindingState == FindingContentState.ImmovableDiagonalNeighbour ||
                        westContentFindingState == FindingContentState.BeyondTheBoard)
                    {
                        continueFindInWest = false;
                    }
                }
            }

            resultCell = null;
            route = null;
            return FindingContentState.DoesNotExist;
        }

        private FindingContentState GetCellFromDiagonallyUpwardNeighbourOrNeighboursAboveCell(
            in Vector2 emptyCellPosition,
            Direction upwardDirection,
            out Cell resultCell,
            out Vector2 beforeImmovablePos)
        {
            beforeImmovablePos = Vector2.positiveInfinity;
            resultCell = null;
            if (_cellCollection.TryGetCellGoesDiagonallyUpwards(emptyCellPosition, upwardDirection,
                out Cell cell))
            {
                bool contentIsImmovable = !cell.Content.Switchable && cell.Content.MatchType != DefaultContentType.Empty;
                if (contentIsImmovable)
                    return FindingContentState.ImmovableDiagonalNeighbour;

                if (cell.Content.MatchType != DefaultContentType.Empty && !cell.Content.IsFalling)
                {
                    resultCell = cell;
                    return FindingContentState.FoundMovable;
                }

                return GetFilledCellAbove(cell.Position, out resultCell, out beforeImmovablePos);
            }

            return FindingContentState.BeyondTheBoard;
        }

        private FindingContentState GetFilledCellAbove(in Vector2 position, out Cell filledCell,
            out Vector2 beforeImmovablePos)
        {
            beforeImmovablePos = Vector2.positiveInfinity;
            filledCell = null;
            var currentPosition = position;
            while (_cellCollection.TryGetCellAbove(currentPosition, out var cellAbove))
            {
                if (cellAbove.Content.MatchType == DefaultContentType.Empty ||
                    cellAbove.Content.IsFalling)
                {
                    currentPosition = cellAbove.Position;
                    continue;
                }

                if (cellAbove.Content.Switchable)
                {
                    filledCell = cellAbove;
                    return FindingContentState.FoundMovable;
                }

                beforeImmovablePos = currentPosition + new Vector2(0, -1);
                return FindingContentState.FoundImmovable;
            }

            return FindingContentState.GenerateNew;
        }

        private void ResetSpawnPositionsArray()
        {
            for (int i = 0; i < _ySpawnPositions.Length; i++)
                _ySpawnPositions[i] = YMinSpawnPosition;
        }

        enum FindingContentState
        {
            FoundMovable,
            FoundImmovable,
            GenerateNew,
            BeyondTheBoard,
            ImmovableDiagonalNeighbour,
            DoesNotExist
        }
    }
}
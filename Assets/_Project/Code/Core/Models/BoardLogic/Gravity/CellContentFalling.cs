﻿using System;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Directions;
using _Project.Code.Core.Models.Random;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Gravity
{
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

            var aboveContentType = GetFilledCellAbove(emptyCell.Position, out Cell filledAboveCell);

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
                var contentStateInDiagonalFinding = GetFilledCellInDiagonal(emptyCell.Position,
                    out Cell filledDiagonalCell, out ContentRoute route);

                if (contentStateInDiagonalFinding == FindingContentState.DoesNotExist)
                    return false;

                if (contentStateInDiagonalFinding == FindingContentState.FoundMovable)
                {
                    if (filledDiagonalCell == null)
                    {
                        UnityEngine.Debug.Log("f");
                    }
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
                new Vector2(to.Position.x, _ySpawnPositions[xIndexOnMatrixOfEmptyCell]));

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
            out ContentRoute route)
        {
            bool continueFindInWest = true;
            Vector2 westPosition = emptyCellPosition;
            ContentRoute westRoute = new ContentRoute(targetPosition: emptyCellPosition);

            bool continueFindInEast = true;
            Vector2 eastPosition = emptyCellPosition;
            ContentRoute eastRoute = new ContentRoute(targetPosition: emptyCellPosition);
            while (continueFindInWest || continueFindInEast)
            {
                if (continueFindInEast)
                {
                    FindingContentState eastContentFindingState =
                        GetCellFromDiagonallyUpwardNeighbourOrNeighboursAboveCell(eastPosition, Direction.East,
                            out resultCell);
                    
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
                            out resultCell);
                    
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
            Direction upwardDirection, out Cell resultCell)
        {
            resultCell = null;
            if (_cellCollection.TryGetCellGoesDiagonallyUpwards(emptyCellPosition, upwardDirection,
                out Cell cell))
            {
                bool contentIsImmovable = !cell.Content.Switchable && cell.Content.MatchType != ContentType.Empty;
                if (contentIsImmovable)
                    return FindingContentState.ImmovableDiagonalNeighbour;
                
                if (cell.Content.MatchType != ContentType.Empty && !cell.Content.IsFalling)
                {
                    resultCell = cell;
                    return FindingContentState.FoundMovable;
                }

                return GetFilledCellAbove(cell.Position, out resultCell);
            }

            return FindingContentState.BeyondTheBoard;
        }

        private FindingContentState GetFilledCellAbove(in Vector2 position, out Cell filledCell)
        {
            filledCell = null;
            var currentPosition = position;
            while (_cellCollection.TryGetCellAbove(currentPosition, out var cellAbove))
            {
                if (cellAbove.Content.MatchType == ContentType.Empty ||
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
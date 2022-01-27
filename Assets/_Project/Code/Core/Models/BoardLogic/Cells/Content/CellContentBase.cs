using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.Framework;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    [Serializable]
    public abstract class CellContentBase : IModel
    {
        private bool _isFalling;
        private Vector2 _position;
        public IEnumerable<DefaultContentType> MatchableContent { get; }
        public bool Switchable { get; }
        public DefaultContentType MatchType { get; }
        public DecoratorType DecoratorType { get; }

        public event Action StartedMovement;

        public event Action PositionChanged;

        public event EventHandler Disabled;

        public event Action Matched;

        public event Action Selected;

        public event Action Deselected;

        public bool IsFalling
        {
            get => _isFalling;
            set
            {
                _isFalling = value;
                if (_isFalling)
                    StartedMovement?.Invoke();
            }
        }

        public bool IsDecorated => GetDecorator().MatchType != DefaultContentType.Empty;

        public Vector2 Position
        {
            get => _position;
            set => ChangePosition(value);
        }

        protected CellContentBase(IEnumerable<DefaultContentType> matchableContent, DefaultContentType matchType, bool switchable,
            DecoratorType decoratorType)
        {
            MatchableContent = matchableContent;
            MatchType = matchType;
            Switchable = switchable;
            DecoratorType = decoratorType;
        }

        public void Deselect() => Deselected?.Invoke();
        public void Select() => Selected?.Invoke();

        public CellContentBase Match()
        {
            Matched?.Invoke();
            return GetDecorator();
        }

        public void Disable() => Disabled?.Invoke(this, EventArgs.Empty);
        protected abstract CellContentBase GetDecorator();

        private void ChangePosition(Vector2 value)
        {
            _position = value;
            PositionChanged?.Invoke();
        }

        public static CellContentBase GetEmptyCached { get; } =
            new DefaultCellContent(Enumerable.Empty<DefaultContentType>(), DefaultContentType.Empty, false);
    }
}
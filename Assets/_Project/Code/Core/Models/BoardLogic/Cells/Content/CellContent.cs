using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.DataStructures;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    [Serializable]
    public class CellContent : IModel, IPoolItem<ContentType>
    {
        private bool _isFalling;
        private readonly ICellContentConfig _config;
        private Vector2 _position;

        public virtual IEnumerable<ContentType> MatchableContent => _config.MatchableContent;

        public virtual bool Switchable => _config.Switchable;

        public virtual ContentType Type => _config.ContentType;

        public event Action StartedMovement;

        public event Action PositionChanged;

        public event EventHandler Disabled;

        public event Action Enabled;

        public event Action Matched;

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

        public Vector2 Position
        {
            get => _position;
            set => ChangePosition(value);
        }
        
        public CellContent(ICellContentConfig config)
        {
            _config = config;
        }

        private void ChangePosition(Vector2 value)
        {
            _position = value;
            PositionChanged?.Invoke();
        }

        
        public CellContent Match()
        {
            Matched?.Invoke();
            return GetDecorator();
        }

        protected virtual CellContent GetDecorator() => EmptyCellContent.GetCached;

        public void Disable() => Disabled?.Invoke(this, EventArgs.Empty);

        public void Enable() => Enabled?.Invoke();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    [Serializable]
    public class CellContent : IModel
    {
        private bool _isFalling;
        private readonly ICellContentConfig _config;
        private Vector2 _position;

        public virtual IEnumerable<ContentType> MatchableContent => _config.MatchableContent;

        public virtual bool Switchable => _config.Switchable;

        public virtual ContentType Type => _config.ContentType;

        public event Action StartedMovement;

        public event Action PositionChanged;

        public event Action Destroyed;

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

        public void Destroy() => Destroyed?.Invoke();
    }

    [Flags]
    public enum ContentType
    {
        Empty,
        Red,
        Blue,
        Orange,
        Purple,
        Green,
        Yellow,
        Stone,
        UppedContent,

        Upped_Red = Red | UppedContent,
        Upped_Blue = Blue | UppedContent,
        Upped_Orange = Orange | UppedContent,
        Upped_Purple = Purple | UppedContent,
        Upped_Green = Green | UppedContent,
        Upped_Yellow = Yellow | UppedContent
    }

    public static class ContentTypeExtensions
    {
        public static ContentType Up(this ContentType contentType)
        {
            if (contentType.HasFlag(ContentType.UppedContent))
                throw new InvalidCastException();
            
            return contentType | ContentType.UppedContent;
        }
    }
}
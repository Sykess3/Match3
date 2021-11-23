using System;
using System.Collections.Generic;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Models.Cells
{
    public class Cell : IModel
    {
        private Content _filler;
        public Content Filler
        {
            get => _filler;
            set
            {
                if (_filler != null) 
                    _filler.Destroyed -= FillerOnDestroyed;

                _filler = value;
                _filler.Destroyed += FillerOnDestroyed;   
            }
        }

        private void FillerOnDestroyed()
        {
            _filler.Destroyed -= FillerOnDestroyed;
            _filler = null;
        }

        public Vector2 Position { get; }

        public Cell(Vector2 position)
        {
            Position = position;
        }


        public class Content : IModel
        {
            private readonly ICellContentConfig _config;
            private Vector2 _position;
            
            public IEnumerable<ContentType> MatchableContent => _config.MatchableContent;
            public bool Switchable => _config.Switchable;
            public ContentType Type => _config.ContentType;
            
            public event Action PositionChanged;
            public event Action Destroyed;

            public Content(ICellContentConfig config)
            {
                _config = config;
            }

            public Vector2 Position
            {
                get => _position;
                set => ChangePosition(value);
            }

            private void ChangePosition(Vector2 value)
            {
                _position = value;
                PositionChanged?.Invoke();
            }

            public void Destroy() => Destroyed?.Invoke();
        }
        public enum ContentType
        {
            Red,
            Blue,
            Orange,
            Purple,
            Green,
            Yellow
        }
    }
}
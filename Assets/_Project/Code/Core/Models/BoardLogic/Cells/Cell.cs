using System;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Framework;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    public class Cell : IModel
    {
        private CellContentBase _content;

        public Vector2 Position { get; }
        public event EventHandler ContentStartedMovement; 

        public CellContentBase Content
        {
            get => _content;
            set => ChangeContent(value);
        }

        public Cell(Vector2 position)
        {
            Position = position;
        }

        /// <summary>
        /// Use this instead of Content.Match()
        /// </summary>
        public void MatchContent()
        {
            var nextContent = Content.Match();
            Content = nextContent;
        }
        
        public void SetContentToEmpty() => Content = CellContentBase.GetEmptyCached;
        
        private void ChangeContent(CellContentBase value)
        {
            if (_content != null) 
                _content.StartedMovement -= OnContentStartedMovement;

            _content = value;
            _content.StartedMovement += OnContentStartedMovement;
        }
        
        private void OnContentStartedMovement() => ContentStartedMovement?.Invoke(this, EventArgs.Empty);

        public void Select() => _content.Select();

        public void Deselect() => _content.Deselect();
    }
}
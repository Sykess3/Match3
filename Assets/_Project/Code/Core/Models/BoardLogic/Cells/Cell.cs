using System;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    public class Cell : IModel
    {
        private CellContent _content;

        public Vector2 Position { get; }
        public event EventHandler ContentStartedMovement; 
        public event EventHandler ContentMatched;

        public CellContent Content
        {
            get => _content;
            set => ChangeContent(value);
        }

        public Cell(Vector2 position)
        {
            Position = position;
        }

        public void SetContentToEmpty() => Content = new EmptyCellContent();

        private void ChangeContent(CellContent value)
        {
            if (_content != null)
            {
                _content.Matched -= ContentOnMatched;
                _content.StartedMovement -= OnContentStartedMovement;
            }

            _content = value;
            _content.Matched += ContentOnMatched;
            _content.StartedMovement += OnContentStartedMovement;
        }

        private void OnContentStartedMovement()
        {
            ContentStartedMovement?.Invoke(this, EventArgs.Empty);
        }


        private void ContentOnMatched()
        {
            _content.Matched -= ContentOnMatched;

            SetContentToEmpty();
            ContentMatched?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
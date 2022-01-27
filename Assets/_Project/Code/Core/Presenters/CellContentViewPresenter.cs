using System;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Presenters.Framework;
using _Project.Code.Core.Views;

namespace _Project.Code.Core.Presenters
{
    public class CellContentViewPresenter : Presenter<DefaultCellContent, CellContentView>
    {
        public CellContentViewPresenter(DefaultCellContent model, CellContentView view) : base(model, view)
        {
        }

        protected override void OnStart()
        {
            SyncPosition();
        }

        protected override void Subscribe()
        {
            Model.Matched += View.Match;
            Model.PositionChanged += SyncPosition;
            Model.Enabled += View.Enable;
            Model.Selected += View.Select;
            Model.Deselected += View.Deselect;
            
            View.AnimationEnded += Model.Disable;
        }

        protected override void CleanUp()
        {
            Model.Matched -= View.Match;
            Model.PositionChanged -= SyncPosition;
            Model.Enabled -= View.Enable;
            Model.Selected -= View.Select;
            Model.Deselected -= View.Deselect;
            
            View.AnimationEnded -= Model.Disable;
        }
        

        private void SyncPosition() => View.transform.position = Model.Position;
    }
}
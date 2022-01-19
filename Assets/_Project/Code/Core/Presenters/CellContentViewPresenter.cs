using System;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Presenters.Framework;
using _Project.Code.Core.Views;

namespace _Project.Code.Core.Presenters
{
    public class CellContentViewPresenter : Presenter<CellContent, CellContentView>
    {
        public CellContentViewPresenter(CellContent model, CellContentView view) : base(model, view)
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
            View.AnimationEnded += Model.Disable;
        }

        protected override void UnSubscribe()
        {
            Model.Matched -= View.Match;
            Model.PositionChanged -= SyncPosition;
            Model.Enabled -= View.Enable;
            View.AnimationEnded -= Model.Disable;
        }
        

        private void SyncPosition() => View.transform.position = Model.Position;
    }
}
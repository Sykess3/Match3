using _Project.Code.Core.Models.BoardLogic.Cells;
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
            View.CellContent = Model;
        }

        protected override void Subscribe()
        {
            Model.Destroyed += View.Match;
            Model.PositionChanged += SyncPosition;
        }

        protected override void UnSubscribe()
        {
            Model.PositionChanged -= SyncPosition;
            Model.Destroyed -= View.Match;
        }

        private void SyncPosition() => View.transform.position = Model.Position;
    }
}
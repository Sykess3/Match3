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
            Model.Matched += View.Match;
            Model.PositionChanged += SyncPosition;
            View.Destroyed += Model.Destroy;
        }

        protected override void UnSubscribe()
        {
            Model.Matched -= View.Match;
            Model.PositionChanged -= SyncPosition;
            View.Destroyed -= Model.Destroy;
        }

        private void SyncPosition() => View.transform.position = Model.Position;
    }
}
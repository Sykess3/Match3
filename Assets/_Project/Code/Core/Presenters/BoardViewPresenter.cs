using _Project.Code.Core.Models;
using _Project.Code.Core.Views;

namespace _Project.Code.Core.Presenters
{
    public class BoardViewPresenter : Presenter<Board, BoardView>
    {
        public BoardViewPresenter(Board model, BoardView view) : base(model, view)
        {
        }

        protected override void OnStart()
        {
            Model.FillCells();
        }

        protected override void UnSubscribe()
        {
            
        }

        protected override void Subscribe()
        {
            
        }
    }
}
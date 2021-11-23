using _Project.Code.Core.Models;
using _Project.Code.Core.Views;

namespace _Project.Code.Core.Presenters
{
    public class BoardViewPresenter : Presenter<Board, BoardView>
    {
        public BoardViewPresenter(Board model, BoardView view) : base(model, view)
        {
        }
    }
}
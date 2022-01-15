using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Presenters;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    public class ContentDecoratorPresenter : CellContentViewPresenter
    {
        public ContentDecoratorPresenter(CellContent model, CellContentView view) : base(model, view)
        {
            
        }

        protected override void OnViewDisable()
        {
            Model.Disable();
            Object.Destroy(View.gameObject);
        }
    }
}
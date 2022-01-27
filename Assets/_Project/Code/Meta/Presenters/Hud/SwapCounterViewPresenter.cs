using _Project.Code.Core.Presenters.Framework;
using _Project.Code.Meta.Models;
using _Project.Code.Meta.Models.Hud;
using _Project.Code.Meta.Views;
using _Project.Code.Meta.Views.Hud;

namespace _Project.Code.Meta.Presenters.Hud
{
    public class SwapCounterViewPresenter : Presenter<SwapCounter, SwapCounterView>
    {
        public SwapCounterViewPresenter(SwapCounter model, SwapCounterView view) : base(model, view)
        {
            
        }

        protected override void OnStart() => View.Refresh(Model.SwapsLeft);

        protected override void Subscribe()
        {
            Model.Subscribe();
            Model.Swapped += View.Refresh;
        }

        protected override void CleanUp()
        {
            Model.CleanUp();
            Model.Swapped -= View.Refresh;
        }
    }
}
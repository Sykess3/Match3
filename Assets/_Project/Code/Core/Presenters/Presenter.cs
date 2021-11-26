using System;
using _Project.Code.Core.Models;
using _Project.Code.Core.Views;

namespace _Project.Code.Core.Presenters
{
    public abstract class Presenter<TModel, TView>
    where TModel : IModel
    where TView : View
    {
        protected readonly TModel Model;
        protected readonly TView View;

        public event EventHandler Destroyed;
        protected Presenter(TModel model, TView view)
        {
            Model = model;
            View = view;
            
            View.Created += OnCreate;
            View.Destroyed += OnDestroy;
        }
        
        
        private void OnCreate()
        {
            if (UnityCallBackFunctionsContractIsCorrect<IUpdatable, UpdatableView>(
                out var updatableModel, out var updatableView))
                updatableView.OnUpdate += updatableModel.Update;

            Subscribe();
            OnStart();
        }

        private void OnDestroy()
        {

            if (UnityCallBackFunctionsContractIsCorrect<IUpdatable, UpdatableView>(
                out var updatableModel, out var updatableView))
                updatableView.OnUpdate -= updatableModel.Update;

            UnSubscribe();
            Destroyed?.Invoke(this, EventArgs.Empty);
        }
        
        private bool UnityCallBackFunctionsContractIsCorrect<TModel_, TView_>(out TModel_ upcastedModel,
            out TView_ upcastedView)
        {
            if (Model is TModel_ model && View is TView_ view)
            {
                upcastedModel = model;
                upcastedView = view;
                return true;
            }

            upcastedModel = default;
            upcastedView = default;

            return false;
        }
        
        protected virtual void UnSubscribe(){}

        protected virtual void Subscribe(){}
        protected virtual void OnStart() {}
    }
}

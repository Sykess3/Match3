using System;
using _Project.Code.Core.Models;
using _Project.Code.Core.Views;
using _Project.Code.Core.Views.Framework;

namespace _Project.Code.Core.Presenters
{
    public abstract class Presenter<TModel, TView>
    where TModel : IModel
    where TView : IView
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
            Subscribe();
            OnStart();
        }

        private void OnDestroy()
        {
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

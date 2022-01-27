using System;
using _Project.Code.Core.Models.Framework;
using _Project.Code.Core.Views.Framework;
using _Project.Code.Meta.Models;
using _Project.Code.Meta.Views;

namespace _Project.Code.Core.Presenters.Framework
{
    public abstract class Presenter<TModel, TView>
        where TModel : IModel
        where TView : IView
    {
        protected readonly TModel Model;
        protected readonly TModel[] Models;

        protected readonly TView View;


        protected Presenter(TModel model, TView view)
        {
            Model = model;
            View = view;

            View.Created += OnCreate;
            View.Destroyed += OnDestroy;
        }

        protected Presenter(TModel[] models, TView view)
        {
            Models = models;
            View = view;
            
            View.Created += OnCreate;
            View.Destroyed += OnDestroy;
        }
        

        private void OnCreate()
        {
            Subscribe();
            OnStart();
        }

        private void OnDestroy() => CleanUp();

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

        protected virtual void CleanUp()
        {
        }

        protected virtual void Subscribe()
        {
        }

        protected virtual void OnStart()
        {
        }
    }
}
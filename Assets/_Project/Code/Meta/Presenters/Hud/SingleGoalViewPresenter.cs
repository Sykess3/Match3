using System;
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Presenters.Framework;
using _Project.Code.Core.Views.Framework;
using _Project.Code.Meta.Models;
using _Project.Code.Meta.Models.Hud;
using _Project.Code.Meta.Views;
using _Project.Code.Meta.Views.Hud;
using TMPro;
using UnityEngine;

namespace _Project.Code.Meta.Presenters.Hud
{
    public class SingleGoalViewPresenter<TEnum> : Presenter<SingleGoal<TEnum>, SingleGoalView> where TEnum : Enum
    {
        public SingleGoalViewPresenter(SingleGoal<TEnum> model, SingleGoalView views) : base(model, views)
        {
            
        }


        protected override void OnStart() => View.UpdateScore(Model.CurrentProgress);

        protected override void Subscribe()
        {
            Model.Subscribe();
            Model.Collected += View.UpdateScore;
        }

        protected override void CleanUp()
        {
            Model.CleanUp();
            Model.Collected -= View.UpdateScore;
        }
    }
}
﻿using _Project.Code.Core.Models;
using _Project.Code.Core.Models.Cells;
using _Project.Code.Core.Views;
using UnityEngine;

namespace _Project.Code.Core.Presenters
{
    public class CellContentViewPresenter : Presenter<CellContent, CellContentView>
    {
        public CellContentViewPresenter(CellContent model, CellContentView view) : base(model, view)
        {
        }

        protected override void OnStart() => SyncPosition();

        protected override void Subscribe()
        {
            Model.Matched += DestroyView;
            Model.PositionChanged += SyncPosition;
        }

        protected override void UnSubscribe()
        {
            Model.PositionChanged -= SyncPosition;
            Model.Matched -= DestroyView;
        }

        
        private void DestroyView() => Object.Destroy(View.gameObject);
        private void SyncPosition() => View.transform.position = Model.Position;
    }
}
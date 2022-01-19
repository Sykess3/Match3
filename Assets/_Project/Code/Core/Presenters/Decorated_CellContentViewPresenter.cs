using System;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Views;

namespace _Project.Code.Core.Presenters
{
    public class Decorated_CellContentViewPresenter : Presenter<Decorator_CellContent, Decorator_CellContentView>
    {
        public Decorated_CellContentViewPresenter(Decorator_CellContent model, Decorator_CellContentView view) : base(model, view)
        {
            
        }
        protected override void OnStart()
        {
            SyncPosition(); //TODO: Кожен декоратор має бути об'єктом якій має свій презентер, отже
                            //треба відписувати на знищенні чи шось типу того але проблема шо в
        }

        protected override void Subscribe()
        {
            Model.Matched += OnMatch;
            Model.PositionChanged += SyncPosition;
            View.AnimationEnded += Model.Disable;
        }

        private void OnMatch()
        {
            
            View.Match();
            UnSubscribe();
        }

        private new void UnSubscribe()
        {
            Model.Matched -= View.Match;
            Model.PositionChanged -= SyncPosition;
            View.AnimationEnded -= Model.Disable;
        }
        
        private void SyncPosition() => View.transform.position = Model.Position;
    }
}
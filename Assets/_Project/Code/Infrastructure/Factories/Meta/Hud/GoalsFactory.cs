using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Infrastructure.Services;
using _Project.Code.Meta.Models;
using _Project.Code.Meta.Models.Hud;
using _Project.Code.Meta.Presenters.Hud;
using _Project.Code.Meta.Views;
using _Project.Code.Meta.Views.Hud;
using _Project.Code.Meta.Views.Hud.Markers;
using UnityEngine;

namespace _Project.Code.Infrastructure.Factories.Meta.Hud
{
    public class GoalsFactory : IGoalsFactory
    {
        private readonly ILevelGoalConfig _levelGoal;
        private readonly IAssetProvider _assetProvider;
        private readonly Board _board;
        private readonly GoalContainer _container;
        
        public GoalsFactory(
            ILevelGoalConfig levelGoal,
            IAssetProvider assetProvider,
            Board board,
            GoalContainer container)
        {
            _levelGoal = levelGoal;
            _assetProvider = assetProvider;
            _board = board;
            _container = container;
        }

        public DefaultSingleGoal[] CreateDefault()
        {
            int index = 0;
            var goals = new DefaultSingleGoal[_levelGoal.DefaultGoal.Count];
            foreach (var config in _levelGoal.DefaultGoal)
            {
                Sprite sprite = _levelGoal.DefaultContentImages[index];
                SingleGoalView view = InstantiateView(_container.transform, sprite);
                DefaultSingleGoal model = new DefaultSingleGoal(_board, (config.Key, config.Value));
                var goalPresenter = new SingleGoalViewPresenter<DefaultContentType>(model, view);

                goals[index] = model;
                index++;
            }

            return goals;
        }

        public DecoratorSingleGoal[] CreateDecorator()
        {
            int index = 0;
            var goals = new DecoratorSingleGoal[_levelGoal.DecoratorsGoal.Count];

            foreach (var config in _levelGoal.DecoratorsGoal)
            {
                Sprite sprite = _levelGoal.DecoratorContentImages[index];
                SingleGoalView view = InstantiateView(_container.transform, sprite);
                DecoratorSingleGoal model = new DecoratorSingleGoal(_board, (config.Key, config.Value));
                var goalPresenter = new SingleGoalViewPresenter<DecoratorType>(model, view);

                goals[index] = model;
                index++;
            }

            return goals;
        }

        private SingleGoalView InstantiateView(Transform transform, Sprite sprite)
        {
            var goalView = _assetProvider.Instantiate(_levelGoal.Prefab, transform);
            goalView.Sprite = sprite;
            return goalView;
        }
    }
}
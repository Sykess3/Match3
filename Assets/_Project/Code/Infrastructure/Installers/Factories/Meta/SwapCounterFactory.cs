using _Project.Code.Core.Input;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Meta.Models.Hud;
using _Project.Code.Meta.Presenters.Hud;
using _Project.Code.Meta.Views.Hud;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Factories.Meta
{
    public class SwapCounterFactory : IFactory<SwapCounter>
    {
        private readonly SwapCounterView _view;
        private readonly BoardInputHandler _boardInputHandler;
        private readonly int _maxSwapsCount;

        public SwapCounterFactory(ILevelConfig levelConfig, SwapCounterView view, BoardInputHandler boardInputHandler)
        {
            _view = view;
            _boardInputHandler = boardInputHandler;
            _maxSwapsCount = levelConfig.MaxStepsCount;
        }

        public SwapCounter Create()
        {
            var model = new SwapCounter(_boardInputHandler, _maxSwapsCount);
            var presenter = new SwapCounterViewPresenter(model, _view);

            return model;
        } 
    }
}
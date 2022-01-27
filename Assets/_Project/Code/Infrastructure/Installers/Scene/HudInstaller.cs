using System;
using _Project.Code.Infrastructure.Factories.Meta.Hud;
using _Project.Code.Infrastructure.Installers.Factories.Meta;
using _Project.Code.Meta.Models;
using _Project.Code.Meta.Models.Hud;
using _Project.Code.Meta.Views.Hud;
using _Project.Code.Meta.Views.Hud.Markers;
using UnityEngine;
using Zenject;

namespace _Project.Code.Infrastructure.Installers.Scene
{
    public class HudInstaller : MonoInstaller
    {
        [SerializeField] private GoalContainer _goalContainer;
        [SerializeField] private SwapCounterView _swapCounter;

        public override void InstallBindings()
        {

            BindMarkers();
            
            Container
                .Bind<IGoalsFactory>()
                .To<GoalsFactory>()
                .AsSingle();
            
            Container
                .Bind<SwapCounter>()
                .FromFactory<SwapCounterFactory>()
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<SwapCounterView>()
                .FromInstance(_swapCounter)
                .AsSingle();
            
            Container
                .Bind(typeof(GoalCalculator), typeof(IInitializable), typeof(IDisposable))
                .To<GoalCalculator>()
                .AsSingle();
        }

        private void BindMarkers()
        {
            Container
                .Bind<GoalContainer>()
                .FromInstance(_goalContainer)
                .AsSingle();
        }
    }
    
}
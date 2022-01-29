using System;
using _Project.Code.Infrastructure.Loading;
using _Project.Code.Meta.Models.UI;
using _Project.Code.Meta.Models.UI.Interfaces;
using _Project.Code.Meta.Views.UI;
using _Project.Code.Meta.Views.UI.Markers;
using UnityEngine;

namespace _Project.Code.Infrastructure.Factories.Meta.UI
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IWindowsConfigs _windowsConfigs;
        private readonly ReloadButton _reloadButton;
        private readonly Transform _root;

        public UIFactory(IAssetProvider assetProvider, IWindowsConfigs windowsConfigs, UIRoot uiRoot, ReloadButton reloadButton)
        {
            _assetProvider = assetProvider;
            _windowsConfigs = windowsConfigs;
            _reloadButton = reloadButton;
            _root = uiRoot.transform;
        }
        
        public void CreateWin() => CreateEndGameWindow(WindowId.Win);

        public void CreateLose() => CreateEndGameWindow(WindowId.Lose);

        private void CreateEndGameWindow(WindowId windowId)
        {
            var prefab = _windowsConfigs.Windows[windowId];
            EndGameWindow endGameWindow = _assetProvider.Instantiate(prefab, _root).GetComponent<EndGameWindow>();
            endGameWindow.RefreshButtonClicked += OnRefreshButtonClick;
        }

        private void OnRefreshButtonClick() => _reloadButton.Reload();
    }
}
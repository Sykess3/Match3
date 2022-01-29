using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Code.Meta.Views.UI
{
    public class EndGameWindow : WindowBase
    {
        [SerializeField] private Button _refreshButton;

        public event Action RefreshButtonClicked;
        
        protected override void OnStart()
        {
            _refreshButton.onClick.AddListener(OnRefreshButtonClick);
        }

        private void OnRefreshButtonClick() => RefreshButtonClicked?.Invoke();
    }
}
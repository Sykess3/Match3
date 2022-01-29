using System;
using _Project.Code.Meta.Models.UI.Interfaces;

namespace _Project.Code.Meta.Models.UI
{
    public class WindowsService
    {
        private readonly IUIFactory _uiFactory;

        public WindowsService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Win:
                    _uiFactory.CreateWin();
                    break;
                case WindowId.Lose:
                    _uiFactory.CreateLose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
            }
        }
    }

    public enum WindowId
    {
        Win,
        Lose
    }
}
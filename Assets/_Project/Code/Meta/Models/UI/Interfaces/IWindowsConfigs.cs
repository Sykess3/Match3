using System.Collections.Generic;
using _Project.Code.Meta.Views.UI;

namespace _Project.Code.Meta.Models.UI.Interfaces
{
    public interface IWindowsConfigs
    {
        Dictionary<WindowId, WindowBase> Windows { get; }
    }
}
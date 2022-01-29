using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Configs;
using _Project.Code.Meta.Models.UI;
using _Project.Code.Meta.Models.UI.Interfaces;
using _Project.Code.Meta.Views.UI;
using UnityEngine;

namespace _Project.Code.Meta.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Windows", order = 0)]
    public class WindowsConfigs : ScriptableObject, IWindowsConfigs
    {
        [SerializeField] private WindowIdAndPrefabPair[] _windows;
        
        public Dictionary<WindowId, WindowBase> Windows => _windows.GetDictionary();
    }

    [Serializable]
    class WindowIdAndPrefabPair : Pair<WindowId, WindowBase>
    {
    }
}
using System;
using _Project.Code.Core.Models;
using UnityEngine;

namespace _Project.Code.Meta.Models.UI
{
    public class ReloadButton
    {
        private readonly ILevelLoader _levelLoader;

        private ReloadButton(ILevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }

        public void Reload() => _levelLoader.Reload();
    }
}
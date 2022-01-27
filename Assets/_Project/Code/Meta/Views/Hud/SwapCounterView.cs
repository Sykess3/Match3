using _Project.Code.Core.Views.Framework;
using TMPro;
using UnityEngine;

namespace _Project.Code.Meta.Views.Hud
{
    public class SwapCounterView : View
    {
        [SerializeField] private TextMeshProUGUI _counter;

        public void Refresh(int count) => _counter.text = count.ToString();
    }
}
using _Project.Code.Core.Views.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Code.Meta.Views.Hud
{
    public class SingleGoalView : View
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        public Sprite Sprite
        {
            set => _image.sprite = value;
        }
        

        public void UpdateScore(int count)
        {
            _text.text = count.ToString();
        }
    }
}
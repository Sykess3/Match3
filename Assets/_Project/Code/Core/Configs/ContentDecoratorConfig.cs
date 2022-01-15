using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/CellContent/Decorator", order = 0)]
    public class ContentDecoratorConfig : ScriptableObject, IContentDecoratorConfig
    {
        [SerializeField] private DecoratorType _type;
        [SerializeField] private bool _switchable;
        [SerializeField] private GameObject _prefab;

        public DecoratorType Type => _type;
        public bool Switchable => _switchable;
        public GameObject Prefab => _prefab;
    }
}
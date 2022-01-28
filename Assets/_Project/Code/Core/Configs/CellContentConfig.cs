using System.Collections.Generic;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/CellContent/Default\\Upped", order = 0)]
    public class CellContentConfig : ScriptableObject, ICellContentConfig
    {
        [SerializeField] private DefaultCellContentView _prefab;
        [SerializeField] private List<DefaultContentType> _matchableContent;
        [SerializeField] private bool _switchable = true;
        [FormerlySerializedAs("_contentType")] [SerializeField] private DefaultContentType defaultContentType;


        public DefaultContentType DefaultContentType => defaultContentType;


        public IEnumerable<DefaultContentType> MatchableContent => _matchableContent;

        public bool Switchable => _switchable;
        public GameObject Prefab => _prefab.gameObject;
    }
}
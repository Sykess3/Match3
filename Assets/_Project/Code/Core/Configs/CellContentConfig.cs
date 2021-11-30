using System.Collections.Generic;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Core.Views;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/CellContent", order = 0)]
    public class CellContentConfig : ScriptableObject, ICellContentConfig
    {
        [SerializeField] private CellContentView _prefab;
        [SerializeField] private ContentType[] _matchableContent;
        [SerializeField] private bool _switchable;
        [SerializeField] private ContentType _contentType;

        public ContentType ContentType => _contentType;
        public IEnumerable<ContentType> MatchableContent => _matchableContent;
        public bool Switchable => _switchable;
        public GameObject Prefab => _prefab.gameObject;
    }
}
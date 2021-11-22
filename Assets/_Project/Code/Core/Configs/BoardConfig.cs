using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "Board", menuName = "StaticData/Board")]
    public class BoardConfig : ScriptableObject, IBoardConfig
    {
        [SerializeField] private Vector2Int _size;
        public Vector2Int Size => _size;
    }
}
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "Board", menuName = "StaticData/Board")]
    public class BoardConfig : ScriptableObject, IBoardConfig
    {
    }
}
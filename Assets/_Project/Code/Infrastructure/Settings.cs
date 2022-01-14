using System.Collections.Generic;
using _Project.Code.Core.Configs;
using UnityEngine;

namespace _Project.Code.Infrastructure
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Initial")]
    public class Settings : ScriptableObject
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private string _firstSceneToLoad;
        //[SerializeField] private CellContentConfig[] _cellContentConfigs;
        
        public LoadingCurtain LoadingCurtain => _loadingCurtain;
        public string FirstSceneToLoad => _firstSceneToLoad;
        //public IEnumerable<CellContentConfig> CellContentConfigs => _cellContentConfigs;
    }
}
using UnityEngine;

namespace _Project.Code.Infrastructure
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Initial")]
    public class Settings : ScriptableObject
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private string _firstSceneToLoad;
        
        public LoadingCurtain LoadingCurtain => _loadingCurtain;
        public string FirstSceneToLoad => _firstSceneToLoad;
    }
}
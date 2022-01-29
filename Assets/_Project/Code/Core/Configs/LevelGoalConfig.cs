using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using _Project.Code.Meta.Views.Hud;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Level/Goal", order = 0)]
    public class LevelGoalConfig : ScriptableObject, ILevelGoalConfig
    {
        [SerializeField] private SingleGoalView _prefab;
        [SerializeField] private ContentGoalPair[] _defaultGoalPairs = new ContentGoalPair[0];
        [SerializeField] private DecoratorGoalPair[] _decoratorGoalPairs = new DecoratorGoalPair[0];
        [SerializeField] private Sprite[] _defaultContentImages;
        [SerializeField] private Sprite[] _decoratedContentImages;

        // Change if console shows errors
        private static string _prefabPath = "Assets/_Project/Resources/Prefabs/Hud/SingleGoal_container.prefab";

        public SingleGoalView Prefab => _prefab;
        public Dictionary<DefaultContentType, int> DefaultGoal => _defaultGoalPairs.GetDictionary();
        public Dictionary<DecoratorType, int> DecoratorsGoal => _decoratorGoalPairs.GetDictionary();
        public Sprite[] DefaultContentImages => _defaultContentImages;
        public Sprite[] DecoratorContentImages => _decoratedContentImages;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_prefab == null)
                _prefab = AssetDatabase.LoadAssetAtPath(_prefabPath, typeof(SingleGoalView)) as SingleGoalView;


            ValidateImages(_decoratorGoalPairs, ref _decoratedContentImages);
            ValidateImages(_defaultGoalPairs, ref _defaultContentImages);
        }

        private void OnEnable() => OnValidate();
        private void OnDisable()
        {
            if (_decoratedContentImages.Any(x => x == null) || _defaultContentImages.Any(x => x == null))
            {
                Debug.LogError($"<color=red>ASSIGN SPRITES TO</color> {name}");
            }
        }

        private void ValidateImages(Array goalPairs, ref Sprite[] imagesArray)
        {
            if (imagesArray.Length > goalPairs.Length)
                Debug.LogError("Pairs count are less than images");

            if (imagesArray.Length != goalPairs.Length)
            {
                Sprite[] newImages = new Sprite[goalPairs.Length];
                for (int i = 0; i < imagesArray.Length; i++)
                    newImages[i] = imagesArray[i];

                imagesArray = newImages;
            }
        }
#endif

        [Serializable]
        class ContentGoalPair : Pair<DefaultContentType, int>
        {
        }

        [Serializable]
        class DecoratorGoalPair : Pair<DecoratorType, int>
        {
        }
    }
}
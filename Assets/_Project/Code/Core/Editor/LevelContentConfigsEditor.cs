using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Project.Code.Core.Configs;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEditor;
using UnityEngine;

namespace _Project.Code.Core.Editor
{
    [CustomEditor(typeof(LevelContentConfigs))]
    public class LevelContentConfigsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelContentConfigs = (LevelContentConfigs) target;

            if (GUILayout.Button("Calculate chances"))
            {
                var contentToSpawn = Calculate(levelContentConfigs.ContentToSpawnTypeChanceMap.Keys.ToArray());
                levelContentConfigs.SetContentToSpawn_Editor(contentToSpawn);
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Collect suitable static data"))
            {
                CollectParticles(ref levelContentConfigs);
                CollectContentConfigs(ref levelContentConfigs);
                EditorUtility.SetDirty(target);
            }
        }

        public static LevelContentConfigs.ContentToSpawnPair[] Calculate(DefaultContentType[] all)
        {
            Dictionary<ContentChanceToSpawn.GenericContentType, float> genericChances =
                ContentChanceToSpawn.CalculateGenericChances(all);
            float previousSum = 0;
            int index = 0;
            var result = new LevelContentConfigs.ContentToSpawnPair[all.Length];
            foreach (var genericTypeChanceKvP in genericChances)
            {
                foreach (var contentType in all)
                {
                    var genericContentType = ContentChanceToSpawn.GetGenericType(contentType);
                    if (genericTypeChanceKvP.Key == genericContentType)
                    {
                        var nextSum = previousSum + genericTypeChanceKvP.Value /
                            ContentChanceToSpawn.GetSubTypesCount(genericTypeChanceKvP.Key);

                        result[index] = new LevelContentConfigs.ContentToSpawnPair
                        {
                            Type = contentType,
                            ChanceToSpawn = new FloatRange(previousSum, nextSum)
                        };

                        previousSum = nextSum;
                        index++;
                    }

                    if (genericContentType == ContentChanceToSpawn.GenericContentType.Immovable)
                    {
                        var spawnPair = result.FirstOrDefault(x => x?.Type == contentType);

                        if (spawnPair == null)
                        {
                            result[index] = new LevelContentConfigs.ContentToSpawnPair
                            {
                                Type = contentType,
                                ChanceToSpawn = new FloatRange(0, 0)
                            };
                            index++;
                        }
                    }
                }
            }

            result.Last().ChanceToSpawn.Max = 1;
            return result;
        }

        private void CollectContentConfigs(ref LevelContentConfigs levelContentConfigs)
        {
            string[] paths = GetCellContentPaths().ToArray();

            CellContentConfig[] allCellContentConfigs = EditorLoadHelper.LoadAssets<CellContentConfig>(paths);

            var resultConfigs = new CellContentConfig[levelContentConfigs.ContentToSpawnTypeChanceMap.Keys.Count];
            int index = 0;
            foreach (var cellContentConfig in allCellContentConfigs)
            {
                foreach (var contentType in levelContentConfigs.ContentToSpawnTypeChanceMap.Keys)
                {
                    if (cellContentConfig.DefaultContentType == contentType)
                    {
                        resultConfigs[index] = cellContentConfig;
                        index++;
                    }
                }
            }
            levelContentConfigs.ContentConfigs = resultConfigs;
        }

        private void CollectParticles(ref LevelContentConfigs levelConfig)
        {
            string[] paths = GetParticlesPaths();

            ParticlesConfig[] particlesConfig = EditorLoadHelper.LoadAssets<ParticlesConfig>(paths);

            var resultParticles = new Dictionary<DefaultContentType, ParticleSystem>();
            foreach (var config in particlesConfig)
            {
                foreach (var contentType in levelConfig.ContentToSpawnTypeChanceMap.Keys)
                {
                    foreach (var particleContentType in config.Particles.Keys)
                    {
                        if (particleContentType == contentType)
                        {
                            resultParticles.Add(contentType, config.Particles[contentType]);
                        }
                    }
                }
            }

            levelConfig.Particles = resultParticles;
        }

        private string[] GetParticlesPaths()
        {
            var particlesDirectoryInfo =
                new DirectoryInfo(Application.dataPath + "/_Project/Resources/StaticData/Particles");

            FileInfo[] files = particlesDirectoryInfo.GetFiles("*.asset", SearchOption.AllDirectories);
            var resultPaths = EditorLoadHelper.SetPathsStartFromAssets(files);

            return resultPaths;
        }

        private List<string> GetCellContentPaths()
        {
            var cellContentDirectoryInfo =
                new DirectoryInfo(Application.dataPath + "/_Project/Resources/StaticData/CellContent");

            var allSubFoldersInfos = cellContentDirectoryInfo.GetDirectories();
            List<string> configsPaths = new List<string>();
            foreach (var subFolderInfo in allSubFoldersInfos)
            {
                var fileInfos = subFolderInfo.GetFiles("*.asset", SearchOption.AllDirectories);
                configsPaths.AddRange(EditorLoadHelper.SetPathsStartFromAssets(fileInfos));
            }

            return configsPaths;
        }
        
    }
}
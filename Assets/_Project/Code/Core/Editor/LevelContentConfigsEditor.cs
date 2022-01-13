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
            }

            if (GUILayout.Button("Collect Particles"))
            {
                CollectParticles(ref levelContentConfigs);
                EditorUtility.SetDirty(target);
            }
        }

        public static LevelContentConfigs.ContentToSpawnPair[] Calculate(ContentType[] all)
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
                    if (genericTypeChanceKvP.Key == ContentChanceToSpawn.GetGenericType(contentType))
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
                }
            }

            result.Last().ChanceToSpawn.Max = 1;
            return result;
        }

        private void CollectParticles(ref LevelContentConfigs levelConfig)
        {
            var resultParticles = new Dictionary<ContentType, ParticleSystem>();
            string[] paths = GetParticlesPaths();

            ParticlesConfig[] particlesConfig = new ParticlesConfig[paths.Length];
            for (int i = 0;
                i < paths.Length;
                i++)
            {
                particlesConfig[i] =
                    AssetDatabase.LoadAssetAtPath(paths[i], typeof(ParticlesConfig)) as ParticlesConfig;
            }

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
            string[] resultPaths = new string[files.Length];

            int dataPathLength = Application.dataPath.Length;
            int charsCountToAssetFolder =
                Application.dataPath.Remove(dataPathLength - "/Assets".Length - 1, "/Assets".Length).Length;

            for (int i = 0; i < files.Length; i++)
            {
                resultPaths[i] = files[i].FullName.Remove(0, charsCountToAssetFolder + 1);
            }

            return resultPaths;
        }
    }
}
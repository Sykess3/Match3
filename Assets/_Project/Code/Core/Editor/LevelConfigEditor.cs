using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models.BoardLogic.Cells;
using UnityEditor;
using UnityEngine;

namespace _Project.Code.Core.Editor
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var levelConfig = (LevelConfig) target;
            if (GUILayout.Button("Collect Particles"))
            {
                CollectParticles(ref levelConfig);
                EditorUtility.SetDirty(target);
            }
        }

        private void CollectParticles(ref LevelConfig levelConfig)
        {
            var resultParticles = new Dictionary<ContentType, ParticleSystem>();
            string[] paths = GetParticlesPaths();
            ParticlesConfig[] particlesConfig = new ParticlesConfig[paths.Length];
            for (int i = 0; i < paths.Length; i++)
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
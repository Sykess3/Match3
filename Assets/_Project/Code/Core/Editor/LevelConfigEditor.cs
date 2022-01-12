﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using _Project.Code.Core.Configs;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Editor
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : UnityEditor.Editor
    {
        private static Vector2 _vectorFieldPosition;
        private static DecoratorType _selectedDecorator;
        private string _positionsToMakeStones_string;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var levelConfig = (LevelConfig) target;

            if (GUILayout.Button("Collect Particles"))
            {
                CollectParticles(ref levelConfig);
                EditorUtility.SetDirty(target);
            }

            using (new GUILayout.HorizontalScope())
            {
                DecoratorsEditing(levelConfig);
            }

            using (new GUILayout.HorizontalScope())
            {
                SetStones(levelConfig);
            }
        }

        private void SetStones(LevelConfig levelConfig)
        {
            _positionsToMakeStones_string = EditorGUILayout.TextField(_positionsToMakeStones_string);
            if (GUILayout.Button("Set stones"))
            {
                var splitedStings = _positionsToMakeStones_string.Split(';');
                foreach (var splitedSting in splitedStings)
                {
                    var position_string = splitedSting.Split(',');
                    if (position_string.Length > 2)
                        Debug.LogError("Wrong format");

                    int.TryParse(position_string[0], NumberStyles.Any, CultureInfo.CurrentCulture, out int x);
                    int.TryParse(position_string[1], NumberStyles.Any, CultureInfo.CurrentCulture, out int y);
                    var position = new Vector2(x, y);
                    levelConfig.ContentEditorSettings
                        .Single(x => x.Position == position)
                        .IsStone = true;
                }
            }
        }

        private static void DecoratorsEditing(LevelConfig levelConfig)
        {
            var cachedLabelWidth = EditorGUIUtility.labelWidth;
            var vectorLabel = "Position";
            EditorGUIUtility.labelWidth = vectorLabel.Length * 6;
            
            _vectorFieldPosition = EditorGUILayout.Vector2Field(vectorLabel, _vectorFieldPosition, 
                GUILayout.MaxWidth(100), GUILayout.MaxWidth(200));
            var popupLabel = "Select decorator";

            EditorGUIUtility.labelWidth = popupLabel.Length * 6;
            _selectedDecorator =
                (DecoratorType) EditorGUILayout.EnumPopup(popupLabel, _selectedDecorator,
                    GUILayout.MinWidth(100));
            if (GUILayout.Button("Set Decorators", GUILayout.Width(100)))
            {
                levelConfig.ContentEditorSettings
                    .Single(x => x.Position == _vectorFieldPosition)
                    .DecoratorType = _selectedDecorator;
            }
            EditorGUIUtility.labelWidth = cachedLabelWidth;
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
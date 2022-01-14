using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Project.Code.Core.Editor
{
    public static class EditorLoadHelper
    {
        public static string[] SetPathsStartFromAssets(FileInfo[] files)
        {
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

        public static T[] LoadAssets<T>(string[] paths) where T : class
        {
            T[] assetsConfigs = new T[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                assetsConfigs[i] =
                    AssetDatabase.LoadAssetAtPath(paths[i], typeof(T)) as T;
            }

            return assetsConfigs;
        }
    }
}
using _Project.Code.Core.Models.BoardLogic;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.BoardLogic.ContentMatching;
using _Project.Code.Core.Models.BoardLogic.Spawner;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Project.Code.Core.Editor
{
    public class BoardEditor : ZenjectEditorWindow
    {
        private DefaultContentType _defaultContentType;
        private DiContainer _domainObjectsContainer;

        [MenuItem("CustomWindow/Board")]
        public static BoardEditor GetOrCreateWindow()
        {
            var window = GetWindow<BoardEditor>();
            window.titleContent = new GUIContent("Board");
            return window;
        }
        
        public override void InstallBindings()
        {
            _domainObjectsContainer = FindObjectOfType<SceneContext>().Container;
        }
        public override void OnGUI()
        {
            base.OnGUI();
            _defaultContentType = (DefaultContentType)EditorGUILayout.EnumPopup("Select type to replace", _defaultContentType);
            if (GUILayout.Button("Create"))
            {
                Vector3 cellPos = Selection.activeGameObject.transform.position;
                var cellCollection = _domainObjectsContainer.Resolve<Board>();
                var cellContentSpawner = _domainObjectsContainer.Resolve<ICellContentSpawner>();

                if (cellCollection.TryGetCell(cellPos, out var cellToReplace))
                {
                    cellToReplace.MatchContent();
                    
                    while (cellToReplace.Content.IsDecorated) 
                        cellToReplace.MatchContent();
                    
                    cellContentSpawner.Spawn(new ContentToSpawn(_defaultContentType, cellPos));
                }
                else
                {
                    Debug.LogWarning("It`s not cell game object");
                }
            }
        }
    }
}
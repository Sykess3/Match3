using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.CustomAttributes;
using _Project.Code.Core.Models;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using _Project.Code.Core.Models.Interfaces.Configs;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Code.Core.Configs
{
    [CreateAssetMenu(fileName = "", menuName = "StaticData/Level/LevelConfig")]
    public class LevelConfig : ScriptableObject, ILevelConfig
    {
        [SerializeField] private LevelContentConfigs _levelContentConfigs;
        [SerializeField] private LevelDecoratorsConfigs _levelDecoratorsConfigs;
        
        [SerializeField] private CellContentEditorSettings[] _contentEditorSettings = 
            new CellContentEditorSettings[Constant.Board.Size.x * Constant.Board.Size.y];
        
        public CellContentEditorSettings[] ContentEditorSettings => _contentEditorSettings;


        public Dictionary<ContentType, float> ContentToSpawnTypeChanceMap =>
            _levelContentConfigs.ContentToSpawnTypeChanceMap;

        public Dictionary<ContentType, ParticleSystem> Particles => _levelContentConfigs.Particles;
        public IEnumerable<ICellContentConfig> CellContentConfigs => _levelContentConfigs.ContentConfigs;

        public IEnumerable<IContentDecoratorConfig> DecoratorsConfigs =>
            _levelDecoratorsConfigs.DecoratorConfigs;

        public DecoratorType GetDecorator(Vector2 position)
        {
            return _contentEditorSettings.Single(PositionsIsEqual).DecoratorType;

            bool PositionsIsEqual(CellContentEditorSettings x)
            {
                return x.Position == position;
            }
        }

        public bool IsStone(Vector2 position)
        {
            return _contentEditorSettings.Single(PositionsIsEqual).IsStone;
            
            bool PositionsIsEqual(CellContentEditorSettings x)
            {
                return x.Position == position;
            }
        }

        [ContextMenu("Reset positions")]
        public void RecalculateBoardEditorSettingsPositions()
        {
            for (int i = 0, index = 0; i < Constant.Board.Size.x; i++)
            {
                for (int j = 0; j < Constant.Board.Size.y; j++, index++)
                {
                    var position = new Vector2(x: i - Constant.Board.OffsetFromCenter.x,
                        y: j - Constant.Board.OffsetFromCenter.y);
                    _contentEditorSettings[index].Position = position;
                }
            }
        }

        [Serializable]
        public class CellContentEditorSettings
        {
            public Vector2 Position;
            public bool IsStone;
            public DecoratorType DecoratorType;
        }
    }
    
}
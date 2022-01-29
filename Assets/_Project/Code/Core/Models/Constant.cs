using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public static class Constant
    {
        public const int MinContentToMatch = 3;
        public const int MinContentToUp = 6;
        public const float Tolerance = 0.01f;
        public const float FallingSpeed = 6f;
        public const float SwapSpeed = 2f;

        public static class Board
        {
            public static Vector2Int Size => new Vector2Int(9, 9);

            public static Vector2 OffsetFromCenter => new Vector2((Size.x - 1)
                                                                  * 0.5f, (Size.y - 1) * 0.5f);

            public static int CellCount => Size.x * Size.y;
        }
    }

    public static class ContentChanceToSpawn
    {
        private const float Default = 0.90f;
        private const float Upped = 0.05f;
        private const float Bomb = 0.01f;

        private const int DefaultSubTypesCount = 6;


        public static Dictionary<GenericContentType, float> CalculateGenericChances(DefaultContentType[] all)
        {
            var genericContentTypes = GetGenericTypesOf(all);
            float sum = 0;
            foreach (var genericContentType in genericContentTypes)
                sum += GetChanceOf(genericContentType);
            
            var result = new Dictionary<GenericContentType, float>();

            foreach (var genericContentType in genericContentTypes)
                result.Add(genericContentType, GetChanceOf(genericContentType) / sum);
            
            return result;
        }

        public enum GenericContentType
        {
            Immovable,
            Default,
            Upped,
            Bomb
        }

        public static GenericContentType GetGenericType(DefaultContentType defaultContentType)
        {
            if (defaultContentType.IsDefault())
                return GenericContentType.Default;
            if (defaultContentType.IsBomb())
                return GenericContentType.Bomb;
            if (defaultContentType.IsUpped())
                return GenericContentType.Upped;


            return GenericContentType.Immovable;
        }

        public static int GetSubTypesCount(GenericContentType genericContentType)
        {
            return DefaultSubTypesCount;
        }

        private static List<GenericContentType> GetGenericTypesOf(DefaultContentType[] all)
        {
            List<GenericContentType> alreadyIncluded = new List<GenericContentType>();
            foreach (var item in all)
            {
                var genericType = GetGenericType(item);
                if (!alreadyIncluded.Contains(genericType) && genericType != GenericContentType.Immovable)
                    alreadyIncluded.Add(genericType);
            }

            return alreadyIncluded;
        }

        private static float GetChanceOf(GenericContentType genericContentType)
        {
            switch (genericContentType)
            {
                case GenericContentType.Default:
                    return Default;
                case GenericContentType.Upped:
                    return Upped;
                case GenericContentType.Bomb:
                    return Bomb;
            }

            throw new ArgumentOutOfRangeException(nameof(genericContentType), genericContentType, null);
        }
    }
}
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
        private const float Default = 0.85f;
        private const float Upped = 0.15f;
        private const float Bomb = 0.08f;

        private const int DefaultSubTypesCount = 6;


        public static Dictionary<GenericContentType, float> CalculateGenericChances(ContentType[] all)
        {
            var genericContentTypes = GetGenericTypesOf(all);
            float sum = 0;
            foreach (var genericContentType in genericContentTypes)
                sum += GetChanceOf(genericContentType);

            if (genericContentTypes.Count == 1)
            {
                return new Dictionary<GenericContentType, float>
                {
                    {genericContentTypes.First(), 1}
                };
            }

            if (sum <= 1)
            {
                var genericChances = new Dictionary<GenericContentType, float>();
                foreach (var genericContentType in genericContentTypes)
                {
                    genericChances.Add(genericContentType, GetChanceOf(genericContentType));
                }

                return genericChances;
            }

            float redundant = sum - 1;
            float redundantByOne = redundant / genericContentTypes.Count;

            var result = new Dictionary<GenericContentType, float>();
            foreach (var genericContentType in genericContentTypes)
            {
                var constantChance = GetChanceOf(genericContentType);
                result.Add(genericContentType, constantChance - redundantByOne);
            }

            return result;
        }

        public enum GenericContentType
        {
            Default,
            Upped,
            Bomb
        }

        public static GenericContentType GetGenericType(ContentType contentType)
        {
            if (contentType.IsDefault())
                return GenericContentType.Default;
            if (contentType.IsBomb())
                return GenericContentType.Bomb;
            if (contentType.IsUpped())
                return GenericContentType.Upped;

            throw new ArgumentOutOfRangeException();
        }

        public static int GetSubTypesCount(GenericContentType genericContentType)
        {
            return DefaultSubTypesCount;
        }

        private static List<GenericContentType> GetGenericTypesOf(ContentType[] all)
        {
            List<GenericContentType> alreadyIncluded = new List<GenericContentType>();
            foreach (var item in all)
            {
                var genericType = GetGenericType(item);
                if (!alreadyIncluded.Contains(genericType))
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
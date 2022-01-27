using System;
using UnityEngine;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public enum DefaultContentType
    {
        Empty,
        Red,
        Blue,
        Orange,
        Purple,
        Green,
        Yellow,
        Stone,

        Upped_Red,
        Upped_Blue,
        Upped_Orange,
        Upped_Purple,
        Upped_Green,
        Upped_Yellow,

        Bomb_Red,
        Bomb_Blue,
        Bomb_Orange,
        Bomb_Purple,
        Bomb_Green,
        Bomb_Yellow,
    }

    public static class ContentTypeExtensions
    {
        private const string Upped = "Upped";
        private const string Bomb = "Bomb";
        public static bool IsUpped(this DefaultContentType defaultContentType) => defaultContentType.ToString().StartsWith(Upped);

        public static bool IsBomb(this DefaultContentType defaultContentType) => defaultContentType.ToString().StartsWith(Bomb);

        public static bool IsDefault(this DefaultContentType defaultContentType)
        {
            return defaultContentType == DefaultContentType.Red || defaultContentType == DefaultContentType.Blue || defaultContentType == DefaultContentType.Green
                   || defaultContentType == DefaultContentType.Orange || defaultContentType == DefaultContentType.Purple ||
                   defaultContentType == DefaultContentType.Yellow;
        }

        public static DefaultContentType GetUppedContent(this DefaultContentType defaultContentType)
        {
            switch (defaultContentType)
            {
                case DefaultContentType.Red:
                    return DefaultContentType.Upped_Red;

                case DefaultContentType.Blue:
                    return DefaultContentType.Upped_Blue;

                case DefaultContentType.Orange:
                    return DefaultContentType.Upped_Orange;

                case DefaultContentType.Purple:
                    return DefaultContentType.Upped_Purple;

                case DefaultContentType.Green:
                    return DefaultContentType.Upped_Green;

                case DefaultContentType.Yellow:
                    return DefaultContentType.Upped_Yellow;
            }

            if (defaultContentType.IsUpped())
                return defaultContentType;

            throw new ArgumentOutOfRangeException();
        }

        public static DefaultContentType GetDefaultBombType(DefaultContentType defaultContentType)
        {
            switch (defaultContentType)
            {
                case DefaultContentType.Bomb_Red:
                    return DefaultContentType.Red;

                case DefaultContentType.Bomb_Blue:
                    return DefaultContentType.Bomb_Blue;

                case DefaultContentType.Bomb_Orange:
                    return DefaultContentType.Orange;

                case DefaultContentType.Bomb_Purple:
                    return DefaultContentType.Purple;

                case DefaultContentType.Bomb_Green:
                    return DefaultContentType.Green;

                case DefaultContentType.Bomb_Yellow:
                    return DefaultContentType.Yellow;
            }

            throw new ArgumentOutOfRangeException(nameof(defaultContentType), defaultContentType, null);
        }

        public static DefaultContentType GetDefaultUppedType(DefaultContentType defaultContentType)
        {
            switch (defaultContentType)
            {
                case DefaultContentType.Upped_Red:
                    return DefaultContentType.Red;

                case DefaultContentType.Upped_Blue:
                    return DefaultContentType.Blue;

                case DefaultContentType.Upped_Orange:
                    return DefaultContentType.Orange;

                case DefaultContentType.Upped_Purple:
                    return DefaultContentType.Purple;

                case DefaultContentType.Upped_Green:
                    return DefaultContentType.Blue;

                case DefaultContentType.Upped_Yellow:
                    return DefaultContentType.Yellow;
            }

            throw new ArgumentOutOfRangeException(nameof(defaultContentType), defaultContentType, null);
        }

        public static DefaultContentType GetBombType(this DefaultContentType defaultContentType)
        {
            switch (defaultContentType)
            {
                case DefaultContentType.Bomb_Red:
                    return DefaultContentType.Red;
                case DefaultContentType.Bomb_Blue:
                    return DefaultContentType.Blue;
                case DefaultContentType.Bomb_Orange:
                    return DefaultContentType.Orange;
                case DefaultContentType.Bomb_Purple:
                    return DefaultContentType.Purple;
                case DefaultContentType.Bomb_Green:
                    return DefaultContentType.Green;
                case DefaultContentType.Bomb_Yellow:
                    return DefaultContentType.Yellow;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int PackCount(this DecoratorType decoratorType)
        {
            if (decoratorType == DecoratorType.None)
                return 0;

            string decoratorInString = decoratorType.ToString();
            var numberInChar = decoratorInString[decoratorInString.Length - 1];
            return numberInChar.ToInt();
        }

        public static DefaultContentType GetAnother(this DefaultContentType defaultContentType)
        {
            int subTypeCount = 6;
            int firstTypeId = GetFirstTypeId(defaultContentType); 
            int lastTypeIdExclusive = firstTypeId + subTypeCount;

            DefaultContentType resultType = GetRandom();
            while (resultType == defaultContentType) 
                resultType = GetRandom();

            return resultType;
            
            DefaultContentType GetRandom()
            {
                return (DefaultContentType)(UnityEngine.Random.Range(firstTypeId, lastTypeIdExclusive));
            }
        }

        public static int GetFirstTypeId(this DefaultContentType defaultContentType)
        {
            if (defaultContentType.IsDefault())
                return (int) DefaultContentType.Red;

            if (defaultContentType.IsUpped())
                return (int) DefaultContentType.Upped_Red;

            if (defaultContentType.IsBomb())
                return (int) DefaultContentType.Bomb_Red;

            throw new ArgumentException();
        }
        

        private static int ToInt(this char character) => character - '0';
    }
}
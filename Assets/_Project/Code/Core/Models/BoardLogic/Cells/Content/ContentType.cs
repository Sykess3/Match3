using System;

namespace _Project.Code.Core.Models.BoardLogic.Cells.Content
{
    public enum ContentType
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
        public static bool IsUpped(this ContentType contentType) => contentType.ToString().StartsWith(Upped);

        public static bool IsBomb(this ContentType contentType) => contentType.ToString().StartsWith(Bomb);

        public static bool IsDefault(this ContentType contentType)
        {
            return contentType == ContentType.Red || contentType == ContentType.Blue || contentType == ContentType.Green
                   || contentType == ContentType.Orange || contentType == ContentType.Purple ||
                   contentType == ContentType.Yellow;
        }

        public static ContentType GetDefault(this ContentType contentType)
        {
            if (contentType.IsBomb())
                return GetDefaultBombType(contentType);
            
            if (contentType.IsUpped())
                return GetDefaultUppedType(contentType);

            return contentType;
        }

        public static ContentType GetUppedContent(this ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Red:
                    return ContentType.Upped_Red;

                case ContentType.Blue:
                    return ContentType.Upped_Blue;
                
                case ContentType.Orange:
                    return ContentType.Upped_Orange;
                
                case ContentType.Purple:
                    return ContentType.Upped_Purple;
                
                case ContentType.Green:
                    return ContentType.Upped_Green;
                
                case ContentType.Yellow:
                    return ContentType.Upped_Yellow;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static ContentType GetDefaultBombType(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Bomb_Red:
                    return ContentType.Red;

                case ContentType.Bomb_Blue:
                    return ContentType.Bomb_Blue;

                case ContentType.Bomb_Orange:
                    return ContentType.Orange;

                case ContentType.Bomb_Purple:
                    return ContentType.Purple;

                case ContentType.Bomb_Green:
                    return ContentType.Green;

                case ContentType.Bomb_Yellow:
                    return ContentType.Yellow;
            }

            throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
        }

        public static ContentType GetDefaultUppedType(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Upped_Red:
                    return ContentType.Red;

                case ContentType.Upped_Blue:
                    return ContentType.Blue;

                case ContentType.Upped_Orange:
                    return ContentType.Orange;

                case ContentType.Upped_Purple:
                    return ContentType.Purple;

                case ContentType.Upped_Green:
                    return ContentType.Blue;

                case ContentType.Upped_Yellow:
                    return ContentType.Yellow;
            }

            throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
        }

        public static ContentType GetBombType(this ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Bomb_Red:
                    return ContentType.Red;
                case ContentType.Bomb_Blue:
                    return ContentType.Blue;
                case ContentType.Bomb_Orange:
                    return ContentType.Orange;
                case ContentType.Bomb_Purple:
                    return ContentType.Purple;
                case ContentType.Bomb_Green:
                    return ContentType.Green;
                case ContentType.Bomb_Yellow:
                    return ContentType.Yellow;
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

        private static int ToInt(this char character) => character - '0';
    }
}
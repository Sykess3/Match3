using System;
using System.Diagnostics;

namespace _Project.Code.Core.Models.BoardLogic.Cells
{
    // public class ContentType
    // {
    //     private const string Upped = "Upped";
    //     private const string Bomb = "Bomb";
    //     private ContentType_Editor _contentType;
    //
    //     public ContentType(ContentType_Editor contentType)
    //     {
    //         _contentType = contentType;
    //     }
    //     
    //
    //     public static ContentType Empty { get; } = new ContentType(ContentType_Editor.Empty);
    //
    //     public static ContentType Stone { get; } = new ContentType(ContentType_Editor.Stone);
    //
    //     public void Up()
    //     {
    //         switch (_contentType)
    //         {
    //             case ContentType_Editor.Red:
    //                 _contentType = ContentType_Editor.Upped_Red;
    //                 break;
    //             case ContentType_Editor.Blue:
    //                 _contentType = ContentType_Editor.Upped_Blue;
    //                 break;
    //             case ContentType_Editor.Orange:
    //                 _contentType = ContentType_Editor.Upped_Orange;
    //                 break;
    //             case ContentType_Editor.Purple:
    //                 _contentType = ContentType_Editor.Upped_Purple;
    //                 break;
    //             case ContentType_Editor.Green:
    //                 _contentType = ContentType_Editor.Upped_Green;
    //                 break;
    //             case ContentType_Editor.Yellow:
    //                 _contentType = ContentType_Editor.Upped_Yellow;
    //                 break;
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //     }
    //
    //     public ContentType GetBombType()
    //     {
    //         switch (_contentType)
    //         {
    //             case ContentType_Editor.Bomb_Red:
    //                 return new ContentType(ContentType_Editor.Red);
    //             case ContentType_Editor.Bomb_Blue:
    //                 return new ContentType(ContentType_Editor.Blue);
    //             case ContentType_Editor.Bomb_Orange:
    //                 return new ContentType(ContentType_Editor.Orange);
    //             case ContentType_Editor.Bomb_Purple:
    //                 return new ContentType(ContentType_Editor.Purple);
    //             case ContentType_Editor.Bomb_Green:
    //                 return new ContentType(ContentType_Editor.Green);
    //             case ContentType_Editor.Bomb_Yellow:
    //                 return new ContentType(ContentType_Editor.Yellow);
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //     }
    //
    //
    //     public override bool Equals(object obj) => ((ContentType) obj) == this;
    //
    //     public override int GetHashCode() => _contentType.GetHashCode();
    //
    //     public static bool operator ==(ContentType first, ContentType second)
    //     {
    //         if (first is null)
    //             return second is null;
    //         
    //         if (second is null)
    //             return true;
    //
    //         return first._contentType == second._contentType;
    //     }
    //
    //     public static bool operator !=(ContentType first, ContentType second) => !(first == second);
    // }

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
        public static bool IsUpped(this ContentType contentType) => contentType.ToString().Contains(Upped);

        public static bool IsBomb(this ContentType contentType) => contentType.ToString().Contains(Bomb);
        

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
    }
}
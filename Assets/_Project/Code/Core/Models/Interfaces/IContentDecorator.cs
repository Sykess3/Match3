using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Core.Models.Interfaces
{
    public interface IContentDecorator
    {
        CellContent Decorate(CellContent contentToDecorate, DecoratorType type);
    }
}
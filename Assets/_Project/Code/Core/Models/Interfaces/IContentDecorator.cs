using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Core.Models.Interfaces
{
    public interface IContentDecorator
    {
        CellContentBase Decorate(CellContentBase contentBaseToDecorate, DecoratorType type);
    }
}
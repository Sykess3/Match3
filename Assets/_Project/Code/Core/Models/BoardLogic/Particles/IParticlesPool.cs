using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Models.BoardLogic.Cells.Content;

namespace _Project.Code.Core.Models.BoardLogic.Particles
{
    public interface IParticlesPool
    {
        ParticlesWrapper Get(DefaultContentType type);
    }
}
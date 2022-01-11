using _Project.Code.Core.Models.BoardLogic.Cells;

namespace _Project.Code.Core.Models.BoardLogic.Particles
{
    public interface IParticlesPool
    {
        ParticlesWrapper Get(ContentType type);
    }
}
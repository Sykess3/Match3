namespace _Project.Code.Infrastructure.GameStates.Interfaces
{
    public interface IGameState : IExitableGameState
    {
        void Enter();
    }
}
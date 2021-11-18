namespace _Project.Code.Infrastructure.GameStates.Interfaces
{
    public interface IGameStateWithPayload<in TPayload> : IExitableGameState
    {
        void Enter(TPayload payload);
    }
}
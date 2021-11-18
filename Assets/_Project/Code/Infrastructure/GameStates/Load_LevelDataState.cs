using _Project.Code.Infrastructure.GameStates.Interfaces;

namespace _Project.Code.Infrastructure.GameStates
{
    public class Load_LevelDataState : IGameStateWithPayload<string>
    {
        public void Enter(string payload)
        {
            //TODO:LoadLevelData
        }

        public void Exit()
        {
        }
    }
}

namespace _Project.Code.Core.Views.Animation
{
    public interface IAnimatorStateReader
    {
        void EnteredState(int hash, float playDuration);
        void ExitedState(int hash, float playDuration);
    }
}
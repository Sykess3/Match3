using UnityEditor.Animations;

namespace _Project.Code.Core.Views.Animation
{
    public interface IAnimatorStateReader
    {
        void EnteredState(int hash);
        void ExitedState(int hash);
    }
}
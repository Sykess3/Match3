using System;
using _Project.Code.Core.Views.Framework;

namespace _Project.Code.Core.Views
{
    public interface ICellContentView
    {
        event Action AnimationEnded;
        event Action Matched;
        void Match();
    }
}
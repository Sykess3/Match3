namespace _Project.Code.Meta.Models.Hud
{
    public interface IGoalsFactory
    {
        DefaultSingleGoal[] CreateDefault();
        DecoratorSingleGoal[] CreateDecorator();
    }
}
namespace _Project.Code.Core.Models.Interfaces.Configs
{
    public interface IBombConfig : ICellContentConfig

    {
    int MaxContentAmountToDestroy { get; }
    int MinContentAmountToDestroy { get; }
    }
}